namespace dealstealunreal.com.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;
    using Data.Interfaces;

    using dealstealunreal.com.Infrastructure.Processing.Interfaces;

    using Exceptions;
    using Infrastructure.Sessions.Interfaces;
    using Models;
    using Models.Deals;
    using Models.User;
    using Models.Wrappers;

    public class DealController : Controller
    {
        private readonly IDealDataAccess dealDataAccess;
        private readonly ISessionController sessionController;
        private readonly IMemberDataAccess memberDataAccess;
        private readonly ICommentDataAccess commentDataAccess;
        private readonly IVoteDataAccess voteDataAccess;
        private readonly IVoteProcessor voteProcessor;

        public DealController(IDealDataAccess dealDataAccess, ISessionController sessionController, IMemberDataAccess memberDataAccess, ICommentDataAccess commentDataAccess, IVoteDataAccess voteDataAccess, IVoteProcessor voteProcessor)
        {
            this.dealDataAccess = dealDataAccess;
            this.sessionController = sessionController;
            this.memberDataAccess = memberDataAccess;
            this.commentDataAccess = commentDataAccess;
            this.voteDataAccess = voteDataAccess;
            this.voteProcessor = voteProcessor;
        }

        public ActionResult Index()
        {
            return View("DealProfile");
        }

        public ActionResult DealProfile(string userId = null)
        {
            User user = GetCurrentUser();

            User userFromId = null;

            try
            {
                userFromId = memberDataAccess.GetUser(userId);
            }
            catch (MemberDatabaseException e)
            {
                // TODO: Log this exception
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
                //TODO: Log that DB is down
            }

            User notNullUser = userFromId ?? user;

            UserDeals deal = new UserDeals()
                {
                    User = notNullUser,
                    Deals = userDeals.Where(a => a.UserName.Trim().ToLower().Equals(notNullUser.UserName.ToLower().Trim())).ToList(),
                    IsCurrentUser = notNullUser.UserName.ToLower().Trim().Equals(user != null ? user.UserName.ToLower().Trim() : string.Empty)
                };

            return PartialView(deal);
        }

        public ActionResult Deals()
        {
            List<Deal> deals = new List<Deal>();
            User user = GetCurrentUser();

            try
            {
                deals = dealDataAccess.GetAllDeals().ToList();

                foreach (Deal deal in deals)
                {
                    int votes = voteDataAccess.GetVotes(deal.DealID);
                    deal.Votes = voteProcessor.CalculateVote(votes);
                    deal.CanVote = voteDataAccess.CanVote(deal.DealID, user == null ? string.Empty : user.UserName);
                }
            }
            catch (DealDatabaseException)
            {
                //TODO: Log that DB is down
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
        public ActionResult SubmitDeal(Deal deal)
        {
            if (ModelState.IsValid)
            {
                deal.UserName = this.GetCurrentUser().UserName;

                deal.ImageUrl = this.UrlExists(deal.ImageUrl) ? deal.ImageUrl : Request.Url.Authority + Url.Content("~/images/deal.png");

                try
                {
                    dealDataAccess.SaveDeal(deal);
                }
                catch (DealDatabaseException)
                {
                    ModelState.AddModelError("System", "An error occurred when saving your deal - please try again later");
                }
            }

            return this.View(deal);
        }

        private bool UrlExists(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                // TODO: Log that the image didn't exist
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
                    User user = this.GetCurrentUser();

                    Deal deal = dealDataAccess.GetDeal(dealId);

                    if (user.UserName != deal.UserName)
                    {
                        voteDataAccess.AddVote(dealId, user.UserName, DateTime.Now, vote);
                    }
                    else
                    {
                        // TODO: log attempt!
                    }

                    memberDataAccess.AddPoint(deal.UserName);
                }
                catch (DealDatabaseException)
                {
                    // TODO: log error
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
                //TODO: log db is down!
            }
            catch (CommentDatabaseException)
            {
                // TODO: log!
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
                    // TODO: Log!
                }
            }

            User user = GetCurrentUser();

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
            Comment comment = new Comment()
                                  {
                                      CommentString = commentWrapper.NewComment,
                                      Date = DateTime.Now,
                                      DealId = commentWrapper.Deal.DealID,
                                      UserName = this.GetCurrentUser().UserName
                                  };

            try
            {
                commentDataAccess.SaveDealComment(comment);
            }
            catch (CommentDatabaseException)
            {
                ModelState.AddModelError("System", "An error occurred when saving your comment - please try again later");
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
                    User user = this.GetCurrentUser();

                    if (user.UserName == dealDataAccess.GetDeal(dealId).UserName)
                    {
                        dealDataAccess.SaveDealDescription(dealId, dealDescriptionEdit);
                    }
                    else
                    {
                        // TODO: log the attempt!
                    }
                }
                catch (MemberDatabaseException)
                {
                    return RedirectToAction("LogOn", "Account");
                }
                catch (DealDatabaseException)
                {
                    ModelState.AddModelError("System", "An error occurred when saving your description - please try again later");
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
                    User user = this.GetCurrentUser();

                    if (user.UserName == dealDataAccess.GetDeal(dealId).UserName)
                    {
                        dealDataAccess.SaveDealActive(dealId, active);
                    }
                    else
                    {
                        // TODO: log the attempt!
                    }
                }
                catch (MemberDatabaseException)
                {
                    return RedirectToAction("LogOn", "Account");
                }
                catch (DealDatabaseException)
                {
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
                    User user = this.GetCurrentUser();

                    if (user.UserName == dealDataAccess.GetDeal(dealId).UserName)
                    {
                        dealDataAccess.DeleteDeal(dealId);
                    }
                    else
                    {
                        // TODO: log the attempt!
                    }
                }
                catch (MemberDatabaseException)
                {
                    return RedirectToAction("LogOn", "Account");
                }
                catch (DealDatabaseException)
                {
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
                // TODO: log db is down
            }

            IOrderedEnumerable<Deal> orderedDeals = deals.Where(d => d.Date > DateTime.Now.AddDays(-7) && d.Active).OrderByDescending(a => a.Votes);

            List<Deal> topFive = orderedDeals.Take(5).ToList();

            return PartialView(topFive);
        }

        public ActionResult Search(string term)
        {
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
                // TODO: log db is down
            }

            User user = GetCurrentUser();

            return View(new DealList() { Deals = deals, CurrentUsername = user == null ? string.Empty : user.UserName });
        }

        private User GetCurrentUser()
        {
            try
            {
                string Username = sessionController.GetCurrentUser().Username;

                return memberDataAccess.GetUser(Username);
            }
            catch (InvalidSessionException e)
            {
                // TODO: Log this!
            }
            catch (MemberDatabaseException e)
            {
                // TODO: Log this!
            }

            FormsAuthentication.SignOut();

            return null;
        }
    }
}
