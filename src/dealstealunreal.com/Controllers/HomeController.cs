using System.Web.Mvc;

namespace dealstealunreal.com.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Index site
        /// </summary>
        /// <returns>Action</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Join Us
        /// </summary>
        /// <returns>Action</returns>
        public ActionResult JoinTheTeam()
        {
            return View();
        }

        /// <summary>
        /// About Us
        /// </summary>
        /// <returns>Action</returns>
        public ActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// Rewards
        /// </summary>
        /// <returns>Action</returns>
        public ActionResult OurRewardSystem()
        {
            return View();
        }
    }
}
