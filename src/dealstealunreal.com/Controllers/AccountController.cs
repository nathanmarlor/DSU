namespace dealstealunreal.com.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using Data.Interfaces;
    using Exceptions;
    using Infrastructure.Security.Interfaces;
    using Infrastructure.Sessions.Interfaces;
    using Infrastructure.Utilities.Interfaces;
    using Models;
    using Models.Deals;
    using Models.User;
    using Models.Wrappers;
    using Recaptcha;

    public class AccountController : Controller
    {
        private readonly IMemberDataAccess memberDataAccess;
        private readonly ISessionController sessionController;
        private readonly IRecoverPassword forgotPassword;
        private readonly IDealDataAccess dealDataAccess;
        private readonly IHash hash;
        private readonly User user;

        public AccountController(IMemberDataAccess memberDataAccess, ISessionController sessionController, IRecoverPassword forgotPassword, IDealDataAccess dealDataAccess, IHash hash, IUserUtilities userUtils)
        {
            this.memberDataAccess = memberDataAccess;
            this.sessionController = sessionController;
            this.forgotPassword = forgotPassword;
            this.dealDataAccess = dealDataAccess;
            this.hash = hash;

            this.user = userUtils.GetCurrentUser();
        }

        public ActionResult LogOn()
        {
            if (user != null)
            {
                return RedirectToAction("ShowProfile", "Account");
            }

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOn model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (sessionController.Logon(model.UserName, model.Password, model.RememberMe))
                {
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("ShowProfile");
                }

                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            sessionController.Logoff();

            ClearSession();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [RecaptchaControlMvc.CaptchaValidator]
        [HttpPost]
        public ActionResult Register(Register model, bool captchaValid)
        {
            if (ModelState.IsValid)//&& captchaValid)
            {
                // Check if user already exists

                try
                {
                    memberDataAccess.GetUser(model.UserName);
                    ModelState.AddModelError("System", "The username you have chosen is currently in use");
                    return this.View(model);
                }
                catch (MemberDatabaseException)
                {
                    // TODO: Log success
                }

                // Attempt to register the user
                try
                {
                    string pass = model.Password;

                    model.Password = hash.HashString(model.Password);

                    if (model.ProfilePicture != null)
                    {
                        // save avatar
                        string filename = Guid.NewGuid() + model.ProfilePicture.FileName;
                        string pathfile = AppDomain.CurrentDomain.BaseDirectory + "uploads/avatars/" + filename;
                        try
                        {
                            model.ProfilePicture.SaveAs(pathfile);
                        }
                        catch (Exception)
                        {
                            // TODO: Logging!
                        }

                        model.ProfilePicturePath = "~/uploads/avatars/" + filename;
                    }
                    else
                    {
                        model.ProfilePicturePath = "~/images/default_user_profile.jpg";
                    }

                    memberDataAccess.CreateUser(model);

                    sessionController.Logon(model.UserName, pass, false);

                    return PartialView("RegisterSuccess");
                }
                catch (MemberDatabaseException e)
                {
                    ModelState.AddModelError("System", "An error occurred whilst attempting to register - please try again later");
                }
            }
            else if (!captchaValid)
            {
                ModelState.AddModelError("Recaptcha", "The reCAPTCHA wasn't entered correctly. Go back and try it again!");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoverPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    forgotPassword.ResetPassword(model.UsernameOrEmail);
                }
                catch (RecoverPasswordException e)
                {
                    ModelState.AddModelError("System", e.Message);
                }
            }

            return View("RecoverPasswordSuccess");
        }

        public ActionResult EditProfile()
        {
            if (user == null)
            {
                return View("LogOn");
            }

            EditProfile editProfile = new EditProfile()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfilePicturePath = user.ProfilePicture
                };

            return View(editProfile);
        }

        [HttpPost]
        public ActionResult EditProfile(EditProfile model)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return View("LogOn");
                }

                user.Email = model.Email;
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.Password = hash.HashString(model.Password);
                }

                if (model.ProfilePicture != null)
                {
                    // save avatar
                    string filename = Guid.NewGuid() + model.ProfilePicture.FileName;
                    string pathfile = AppDomain.CurrentDomain.BaseDirectory + "uploads/avatars/" + filename;
                    try
                    {
                        model.ProfilePicture.SaveAs(pathfile);
                        user.ProfilePicture = filename;
                    }
                    catch (Exception)
                    {
                        // TODO: Logging!
                    }

                    user.ProfilePicture = "~/uploads/avatars/" + user.ProfilePicture;
                }

                model.ProfilePicturePath = user.ProfilePicture;
                model.UserName = user.UserName;

                try
                {
                    memberDataAccess.UpdateUser(user);
                }
                catch (MemberDatabaseException)
                {
                    ModelState.AddModelError("System", "An error occurred whilst attempting to update your profile - please try again later");
                    return View(model);
                }
            }

            return PartialView("EditProfileSuccess");
        }

        public ActionResult ShowProfile(string userId = null)
        {
            User userFromId = null;
            IList<Deal> deals = new List<Deal>();

            try
            {
                deals = dealDataAccess.GetAllDeals();
                userFromId = memberDataAccess.GetUser(userId);
            }
            catch (MemberDatabaseException e)
            {
                // TODO: Log that the user wasnt found
            }
            catch (DealDatabaseException e)
            {
                // TODO: Could not load deals
            }

            if (userId != null && userFromId == null)
            {
                return this.PartialView("UserDoesNotExist");
            }

            if (user == null && userId == null)
            {
                return PartialView("LogOn");
            }

            User notNullUser = userFromId ?? user;

            UserDeals deal = new UserDeals()
            {
                User = notNullUser,
                Deals = deals.Where(a => a.UserName.Trim().ToLower().Equals(notNullUser.UserName.Trim().ToLower())).ToList(),
                IsCurrentUser = notNullUser.UserName.ToLower().Trim().Equals(user != null ? user.UserName.ToLower().Trim() : string.Empty)
            };

            return View(deal);
        }

        private void ClearSession()
        {
            FormsAuthentication.SignOut();

            RedirectToAction("LogOn", "Account");
        }
    }
}
