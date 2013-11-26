namespace dealstealunreal.com.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Data.Interfaces;
    using Exceptions;
    using Infrastructure.Processing.Interfaces;
    using Infrastructure.Utilities;
    using Models;
    using Models.Deals;
    using Models.User;
    using Models.Wrappers;
    using Ninject.Extensions.Logging;

    public class DealController : Controller
    {
        private readonly ILogger log;
        private readonly IDealDataAccess dealDataAccess;
        private readonly IMemberDataAccess memberDataAccess;
        private readonly ICommentDataAccess commentDataAccess;
        private readonly IVoteDataAccess voteDataAccess;
        private readonly IVoteProcessor voteProcessor;
        private readonly User user;

        public DealController(ILogger log, IDealDataAccess dealDataAccess, IMemberDataAccess memberDataAccess, ICommentDataAccess commentDataAccess, IVoteDataAccess voteDataAccess, IVoteProcessor voteProcessor, ICurrentUser currentUser)
        {
            this.log = log;
            this.dealDataAccess = dealDataAccess;
            this.memberDataAccess = memberDataAccess;
            this.commentDataAccess = commentDataAccess;
            this.voteDataAccess = voteDataAccess;
            this.voteProcessor = voteProcessor;

            this.user = currentUser.GetCurrentUser();
        }

        public ActionResult Index()
        {
            return View("DealProfile");
        }

        public ActionResult DealProfile(string userId = null)
        {
            User userFromId = null;

            if (userId != null)
            {
                try
                {
                    userFromId = memberDataAccess.GetUser(userId);
                }
                catch (MemberDatabaseException)
                {
                    log.Debug("Could not load user profile for requested user {0}", userId);
                }
            }

            if (user == null && userFromId == null)
            {
                return View("DealProfileUserControl");
            }

            IList<Deal> userDeals = new List<Deal>();

            try
            {
                userDeals = dealDataAccess.GetAllDeals();
            }
            catch (DealDatabaseException)
            {
                log.Warn("Could not get list of deals for user profile");
            }

            User notNullUser = userFromId ?? this.user;

            UserDeals deal = new UserDeals
                {
                    User = notNullUser,
                    Deals = userDeals.Where(a => a.UserName.Equals(notNullUser.UserName)).ToList(),
                    IsCurrentUser = notNullUser.UserName.Equals(user != null ? user.UserName : string.Empty)
                };

            return PartialView(deal);
        }

        public ActionResult Deals()
        {
            IEnumerable<Deal> deals = new List<Deal>();

            try
            {
                deals = dealDataAccess.GetAllDeals();

                foreach (Deal deal in deals)
                {
                    int votes = voteDataAccess.GetVotes(deal.DealID);
                    deal.Votes = voteProcessor.CalculateVote(votes);
                    deal.CanVote = voteDataAccess.CanVote(deal.DealID, user == null ? string.Empty : user.UserName);

                    log.Trace("Found {0} votes for deal {1} - Current user can vote {2}", votes, deal.Title, deal.CanVote);
                }
            }
            catch (DealDatabaseException)
            {
                log.Warn("Could not get list of deals");
            }

            OrderedDeals orderedDeals = new OrderedDeals
                {
                    Deals = deals.Where(a => a.Active).OrderByDescending(a => a.Date),
                    CurrentUsername = user == null ? string.Empty : user.UserName,
                };

            return PartialView(orderedDeals);
        }

        public ActionResult SubmitDeal()
        {
            return PartialView();
        }

        [HttpPost]
        public void SubmitDeal(Deal deal)
        {
            if (ModelState.IsValid)
            {
                deal.UserName = user.UserName;

                if (!string.IsNullOrEmpty(deal.ImageUrl) && !(this.UrlExists(deal.ImageUrl) || deal.ImageUrl.EndsWith(".jpg") || deal.ImageUrl.EndsWith(".png")))
                {
                    log.Debug("The URL {0} specified by {1} is invalid", deal.Url, user.UserName);
                    ModelState.AddModelError("Image URL", "The image URL specified is not valid");
                }
                else
                {
                    if (string.IsNullOrEmpty(deal.ImageUrl))
                    {
                        deal.ImageUrl = "http://" + Request.Url.Authority + Url.Content("/images/deal.png");
                    }

                    deal.Url = deal.Url.StartsWith("http://") ? deal.Url : "http://" + deal.Url;

                    try
                    {
                        dealDataAccess.SaveDeal(deal);
                    }
                    catch (DealDatabaseException)
                    {
                        ModelState.AddModelError("System", "Your deal could not be saved!");
                        log.Warn("Could not save deal to database with title {0}", deal.Title);
                    }
                }
            }

            if (ModelState.Any(a => a.Value.Errors.Any()))
            {
                Response.Clear();
                Response.StatusCode = 500;
                var error = ModelState.Values.First(a => a.Errors.Any()).Errors.First().ErrorMessage;
                log.Debug("Returning error {0} for deal submission from user {1}", error, user.UserName);
                Response.Write(error);
            }
        }

        private bool UrlExists(string url)
        {
            HttpWebResponse response = null;

            log.Trace("Checking if URL {0} exists", url);

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";

                response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch (Exception)
            {
                log.Debug("Image with URL {0} did not exist", url);
                return false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        [HttpPost]
        public ActionResult Voting(int dealId, Vote vote)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Deal deal = dealDataAccess.GetDeal(dealId);

                    if (user.UserName != deal.UserName)
                    {
                        voteDataAccess.AddVote(dealId, user.UserName, DateTime.Now, vote);
                        memberDataAccess.AddPoint(deal.UserName);
                    }
                    else
                    {
                        log.Warn("Attempt to vote for own deal {0} received from user {1}", dealId, user.UserName);
                    }
                }
                catch (DealDatabaseException)
                {
                    log.Warn("Could not get deal {0} from database when saving vote", dealId);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Comment()
        {
            int dealId = int.Parse(RouteData.Values["DealID"].ToString());

            IList<Comment> comments = new List<Comment>();
            Deal deal = new Deal();

            try
            {
                comments = commentDataAccess.GetDealComments(dealId);
                deal = dealDataAccess.GetDeal(dealId);
            }
            catch (DealDatabaseException)
            {
                log.Warn("Could not get deal from database when loading comments");
            }
            catch (CommentDatabaseException)
            {
                log.Warn("Could not get comments from database");
            }

            Dictionary<User, Comment> userComments = new Dictionary<User, Comment>();

            foreach (Comment comment in comments)
            {
                try
                {
                    userComments[memberDataAccess.GetUser(comment.UserName)] = comment;
                }
                catch (MemberDatabaseException)
                {
                    log.Warn("Could not get user from database when loading comments");
                }
            }

            DealComments dealComments = new DealComments()
                {
                    Comments = userComments.OrderByDescending(a => a.Value.Date),
                    Deal = deal,
                    CurrentUsername = user == null ? string.Empty : user.UserName
                };

            return PartialView(dealComments);
        }

        [HttpPost]
        public ActionResult Comment(DealComments commentWrapper)
        {
            Comment comment = new Comment
                                  {
                                      CommentString = commentWrapper.NewComment,
                                      Date = DateTime.Now,
                                      DealId = commentWrapper.Deal.DealID,
                                      UserName = user.UserName
                                  };

            log.Trace("Saving comment {0} for deal {1} from user {2}", commentWrapper.NewComment, commentWrapper.Deal.Title, user.UserName);

            try
            {
                commentDataAccess.SaveDealComment(comment);
            }
            catch (CommentDatabaseException)
            {
                log.Warn("Could not save comment to database");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult EditDescription(int dealId, string dealDescriptionEdit)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (user.UserName == dealDataAccess.GetDeal(dealId).UserName)
                    {
                        dealDataAccess.SaveDealDescription(dealId, dealDescriptionEdit);
                    }
                    else
                    {
                        log.Warn("Attempt to edit description of other users deal received for deal {0} with user {1}", dealId, user.UserName);
                    }
                }
                catch (DealDatabaseException)
                {
                    log.Warn("Could not save deal description for deal {0}", dealId);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult DealActive(int dealId, bool active)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (user.UserName == dealDataAccess.GetDeal(dealId).UserName)
                    {
                        dealDataAccess.SaveDealActive(dealId, active);
                    }
                    else
                    {
                        log.Warn("Attempt to set other users deal {0} received from {1}", active ? "active" : "inactive", user.UserName);
                    }
                }
                catch (DealDatabaseException)
                {
                    log.Warn("Could not set deal {0} {1}", dealId, active ? "active" : "inactive");
                    ModelState.AddModelError("System", "An error occurred when saving your deal status - please try again later");
                }
            }

            return RedirectToAction("ShowProfile", "Account");
        }

        [HttpPost]
        public ActionResult DealDelete(int dealId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (user.UserName == dealDataAccess.GetDeal(dealId).UserName)
                    {
                        dealDataAccess.DeleteDeal(dealId);
                    }
                    else
                    {
                        log.Warn("Attempt to delete other users deal {0} received from user {1}", dealId, user.UserName);
                    }
                }
                catch (DealDatabaseException)
                {
                    log.Warn("Could not delete deal {0}", dealId);
                    ModelState.AddModelError("System", "An error occurred when deleting your deal - please try again later");
                }
            }

            return RedirectToAction("ShowProfile", "Account");
        }

        public ActionResult ShowTopFive()
        {
            IList<Deal> deals = new List<Deal>();

            try
            {
                deals = dealDataAccess.GetAllDeals();
            }
            catch (DealDatabaseException)
            {
                log.Warn("Could not load list of deals from database to show top five");
            }

            foreach (Deal deal in deals)
            {
                int votes = voteDataAccess.GetVotes(deal.DealID);
                deal.Votes = voteProcessor.CalculateVote(votes);
                log.Trace("Received {0} votes for deal {1}", votes, deal.Title);
            }

            IOrderedEnumerable<Deal> orderedDeals = deals.Where(d => d.Date > DateTime.Now.AddDays(-7) && d.Active).OrderByDescending(a => a.Votes);

            List<Deal> topFive = orderedDeals.Take(5).ToList();

            return PartialView(topFive);
        }

        public ActionResult Search(string term)
        {
            log.Trace("Searching for deal with term {0}", term);

            if (term.Equals(string.Empty))
            {
                RedirectToAction("Index", "Home");
            }

            IList<Deal> deals = new List<Deal>();

            try
            {
                deals = dealDataAccess.SearchForDeal(term);
            }
            catch (DealDatabaseException)
            {
                log.Warn("Could not load deals from database when searching");
            }

            foreach (Deal deal in deals)
            {
                int votes = voteDataAccess.GetVotes(deal.DealID);
                deal.Votes = voteProcessor.CalculateVote(votes);
                deal.CanVote = voteDataAccess.CanVote(deal.DealID, user == null ? string.Empty : user.UserName);
                log.Debug("Found {0} votes for deal {1} - Current user can vote {2}", votes, deal.Title, deal.CanVote);
            }

            return View(new DealList { Deals = deals, CurrentUsername = user == null ? string.Empty : user.UserName });
        }
    }
}
