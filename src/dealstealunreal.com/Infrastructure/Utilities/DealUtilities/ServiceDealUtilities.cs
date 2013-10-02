//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using dealstealunreal.com.Models;
//using dealstealunreal.com.Utils.Membership;
//using System.Configuration;
//using System.Web.Routing;
//using System.Net.Mail;

//namespace dealstealunreal.com.Utils.DealUtilities
//{
//    public class ServiceDealUtilities : IDealUtilities
//    {
//        public void SaveVote(Vote vote, string DealID)
//        {
//            Models.Voting v = new Voting();
//            v.Date = DateTime.Now;
//            v.DealID = Guid.Parse(DealID);
//            v.UserID = Guid.Parse(Member.GetUser().UserID.ToString());
//            switch (vote)
//            {
//                case Vote.NegativeVote:
//                    v.NegativeVote = 1;
//                    break;
//                case Vote.PositiveVote:
//                    v.PositiveVote = 1;
//                    break;
//            }
//            Dsu.Votings.InsertOnSubmit(v);

//            if (vote == Vote.PositiveVote)
//            {
//                int numVote = Utilities.CalculateVote(v.DealID);
//                var dealdb = Dsu.Deals.SingleOrDefault(d => d.DealID.Equals(v.DealID));
//                string emailAddr = Member.GetUser(dealdb.DealUserID.Value).Email;
//                MailMessage mail = new MailMessage();
//                mail.To.Add(emailAddr);
//                if (numVote >= 20 && numVote < 40 && dealdb.DealEmail == null)
//                {
//                    try
//                    {
//                        mail.Subject = "New level - Deal";
//                        mail.Body = "Congrats your deal reached DEAL!";
//                        Utilities.SendMail(mail);
//                    }
//                    catch (Exception)
//                    {
                        
//                    }
//                    dealdb.DealEmail = "D";
//                }
//                else if (numVote >= 40 && numVote < 60 && (dealdb.DealEmail.Equals("D") || dealdb.DealEmail == null))
//                {
//                    try
//                    {
//                        mail.Subject = "New level - Steal";
//                        mail.Body = "Congrats your deal reached STEAL!";
//                        Utilities.SendMail(mail);
//                    }
//                    catch (Exception)
//                    {
                        
//                    }
//                    dealdb.DealEmail = "S";
//                }
//                else if (numVote >= 60 && (dealdb.DealEmail.Equals("S") || dealdb.DealEmail.Equals("D") || dealdb.DealEmail == null))
//                {
//                    try
//                    {
//                        mail.Subject = "New level - Unreal";
//                        mail.Body = "Congrats your deal reached UNREAL!";
//                        Utilities.SendMail(mail);
//                    }
//                    catch (Exception)
//                    {
                       
//                    }
//                    dealdb.DealEmail = "U";
//                }
//            }
//            Dsu.SubmitChanges();
//        }


//        public CommentsModel CommentList(string DealID)
//        {
//            string path = "~/uploads/avatars/";
//            CommentsModel model = new CommentsModel();
//            if (Member.IsAuthenticated)
//                model.CurrentUserName = Member.GetUser().UserName;
//            model.DealID = DealID;
//            var comments = from c in Dsu.Comments join u in Dsu.Users on c.UserID equals u.UserId join up in Dsu.UserProfiles on c.UserID equals up.UserId where c.DealID.Equals(model.DealID) orderby c.Date descending select new { Comment = c, u.UserName, up.ProfilePicture };
//            foreach (var comment in comments)
//            {
//                CommentModel cmt = new CommentModel();
//                cmt.Comment = comment.Comment.CommentContent;
//                cmt.Date = comment.Comment.Date.GetValueOrDefault();
//                cmt.UserID = comment.Comment.UserID.ToString();
//                cmt.UserName = comment.UserName;
//                if (comment.ProfilePicture == null)
//                {
//                    cmt.UserAvatar = VirtualPathUtility.ToAppRelative("~/images/default_user_profile.jpg");
//                }
//                else
//                {
//                    cmt.UserAvatar = VirtualPathUtility.ToAppRelative(path + comment.ProfilePicture);
//                }
//                model.Comments.Add(cmt);
//            }
//            return model;
//        }

//        public void SaveComment(CommentsModel model)
//        {
//            Comment comment = new Comment();
//            comment.CommentContent = model.Comment;
//            comment.Date = DateTime.Now;
//            comment.DealID = Guid.Parse(model.DealID);
//            comment.UserID = Guid.Parse(Member.GetUser().UserID.ToString());
//            Dsu.Comments.InsertOnSubmit(comment);
//            Dsu.SubmitChanges();
//        }

//        public void EditDescription(string DealID, string DealDescriptionEdit)
//        {
//            Deal deal = Dsu.Deals.Single(d => d.DealID.Equals(DealID));
//            if (deal.DealUserID.Equals(Member.CurrentUserID))
//            {
//                deal.DealDescription = DealDescriptionEdit;
//                Dsu.SubmitChanges();
//            }
//        }

//        public void DealAction(string Act, string DealID)
//        {
//            Deal deal = Dsu.Deals.Single(d => d.DealID.Equals(DealID));
//            if (deal.DealUserID.Equals(Member.CurrentUserID))
//            {
//                switch (Act)
//                {
//                    case "del":
//                        Dsu.Deals.DeleteOnSubmit(deal);
//                        break;
//                    case "deactive":
//                        deal.Status = 0;
//                        break;
//                    case "active":
//                        deal.Status = 1;
//                        break;
//                }
//                Dsu.SubmitChanges();
//            }
//        }

//        public List<Deal> GetTopFive()
//        {
//            var weekDeals = (from d in Dsu.Deals where d.Status == 1 && d.Date >= DateTime.Now.AddDays(-7) orderby d.Date descending select d).ToList();
//            SortedDictionary<Guid, int> dic = new SortedDictionary<Guid, int>();
//            foreach (var deal in weekDeals)
//            {
//                int numVote = Utilities.CalculateVote(deal.DealID);
//                dic.Add(deal.DealID, numVote);
//            }
//            var top = dic.OrderByDescending(s => s.Value).Take(5);

//            var deals = (from d in weekDeals join t in top on d.DealID equals t.Key orderby t.Value select d).ToList();

//            return deals;
//        }

//        public DealsModel SearchDeal(string Term)
//        {
//            string[] matches = Term.Split(new string[] { "+", " ", "\"", "'" }, StringSplitOptions.RemoveEmptyEntries);
            
//            DealsModel model = new DealsModel();
//            var deal = from d in Dsu.Deals join u in Dsu.Users on d.DealUserID equals u.UserId where d.Status == 1 orderby d.Date descending select new { Deal = d, u.UserName };
//            foreach (string match in matches)
//            {
//                deal = from d in deal where d.Deal.DealTitle.Contains(match) select d;
//            }
//            IUserUtilities user = Member.GetUser();
//            foreach (var d in deal)
//            {
//                double progress = (double)Utilities.CalculateVote(d.Deal.DealID) * 1.666666667;
//                if (progress > 100)
//                    progress = 100;
//                DealModel dealModel = new DealModel();
//                dealModel.Vote = 0;
//                if (Member.IsAuthenticated)
//                {
//                    dealModel.CurrentUserName = user.UserName;
//                    dealModel.Vote = (from v in Dsu.Votings where v.DealID.Equals(d.Deal.DealID) && v.UserID.Equals(user.UserID) select v).Count();
//                }
//                else
//                    dealModel.CurrentUserName = string.Empty;
//                dealModel.Date = d.Deal.Date.GetValueOrDefault();
//                dealModel.DealID = d.Deal.DealID.ToString();
//                dealModel.Description = d.Deal.DealDescription;
//                dealModel.ImageUrl = d.Deal.DealImageUrl;
//                dealModel.Price = d.Deal.DealPrice;
//                dealModel.Retailer = d.Deal.DealRetailer;
//                dealModel.Title = d.Deal.DealTitle;
//                dealModel.Url = d.Deal.DealUrl;
//                dealModel.UserID = d.Deal.DealUserID.ToString();
//                dealModel.UserName = d.UserName;
//                dealModel.Progress = progress;
//                model.Deals.Add(dealModel);
//            }

//            return model;
//        }
//    }
//}