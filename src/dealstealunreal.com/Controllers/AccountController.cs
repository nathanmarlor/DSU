﻿namespace dealstealunreal.com.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using Data.Interfaces;
    using Exceptions;
    using Infrastructure.Communication.Interfaces;
    using Infrastructure.Security.Interfaces;
    using Infrastructure.Sessions.Interfaces;
    using Infrastructure.Utilities;
    using Models;
    using Models.Deals;
    using Models.User;
    using Models.Wrappers;
    using Ninject.Extensions.Logging;
    using Recaptcha;

    /// <summary>
    /// Account controller
    /// </summary>
    public class AccountController : Controller
    {
        private readonly ILogger log;
        private readonly IMemberDataAccess memberDataAccess;
        private readonly ISessionController sessionController;
        private readonly IRecoverPassword forgotPassword;
        private readonly IDealDataAccess dealDataAccess;
        private readonly IHash hash;
        private readonly IEmailSender emailSender;
        private readonly User user;

        /// <summary>
        /// Initialises a new instance of the <see cref="AccountController"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        /// <param name="memberDataAccess">Member data access</param>
        /// <param name="sessionController">Session controller</param>
        /// <param name="forgotPassword">Forgot password</param>
        /// <param name="dealDataAccess">Deal data access</param>
        /// <param name="hash">Hasher</param>
        /// <param name="emailSender">Email sender</param>
        /// <param name="currentUser">Current user</param>
        public AccountController(ILogger log, IMemberDataAccess memberDataAccess, ISessionController sessionController, IRecoverPassword forgotPassword, IDealDataAccess dealDataAccess, IHash hash, IEmailSender emailSender, ICurrentUser currentUser)
        {
            this.log = log;
            this.memberDataAccess = memberDataAccess;
            this.sessionController = sessionController;
            this.forgotPassword = forgotPassword;
            this.dealDataAccess = dealDataAccess;
            this.hash = hash;
            this.emailSender = emailSender;

            user = currentUser.GetCurrentUser();
        }

        /// <summary>
        /// GET request to logon
        /// </summary>
        /// <returns>Userprofile</returns>
        public ActionResult LogOn()
        {
            if (user != null)
            {
                return RedirectToAction("ShowProfile", "Account");
            }

            return View();
        }

        /// <summary>
        /// POST request to logon
        /// </summary>
        /// <param name="model">Logon model</param>
        /// <returns>User profile</returns>
        [HttpPost]
        public ActionResult LogOn(LogOn model)
        {
            if (ModelState.IsValid)
            {
                if (sessionController.Logon(model.UserName, model.Password, model.RememberMe))
                {
                    log.Debug("Successfully logged on user: {0}", model.UserName);
                    return RedirectToAction("ShowProfile");
                }

                log.Debug("Could not log on user: {0} pass: {1}", model.UserName, model.Password);
                ModelState.AddModelError("System", "The user name or password provided is incorrect.");
            }

            return View(model);
        }

        /// <summary>
        /// GET request to log off
        /// </summary>
        /// <returns>Home</returns>
        public ActionResult LogOff()
        {
            sessionController.Logoff();

            log.Trace("Clearing session for user: {0}", user.UserName);

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET register
        /// </summary>
        /// <returns>Register page</returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST register
        /// </summary>
        /// <param name="model">Register model</param>
        /// <param name="captchaValid">Captcha is valid</param>
        /// <returns>Action</returns>
        [RecaptchaControlMvc.CaptchaValidator]
        [HttpPost]
        public ActionResult Register(Register model, bool captchaValid)
        {
            if (ModelState.IsValid && captchaValid)
            {
                log.Trace("Attempt to register user: {0} email: {1} profile picture: {2}", model.UserName, model.Email, model.ProfilePicturePath);

                try
                {
                    memberDataAccess.GetUser(model.UserName);

                    log.Debug("Attempt to register user failed, user: {0} already exists", model.UserName);

                    ModelState.AddModelError("System", "The username you have chosen is currently in use");

                    return this.View(model);
                }
                catch (MemberDatabaseException)
                {
                    log.Trace("User {0} does not already exist, can be registered");
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

                        log.Debug("Saving user: {0} profile picture with path: {1}", model.UserName, pathfile);

                        try
                        {
                            model.ProfilePicture.SaveAs(pathfile);
                        }
                        catch (Exception e)
                        {
                            log.Warn(e, "Could not save users: {0} profile picture with path: {1}", model.UserName, pathfile);
                        }

                        model.ProfilePicturePath = "~/uploads/avatars/" + filename;
                    }
                    else
                    {
                        log.Trace("Setting profile picture to default for user: {0}", model.UserName);
                        model.ProfilePicturePath = "~/images/default_user_profile.jpg";
                    }

                    memberDataAccess.CreateUser(model);

                    sessionController.Logon(model.UserName, pass, false);

                    emailSender.SendEmail(model.Email, "DSU", "Thank you for registering with DealStealUnreal!");

                    return PartialView("RegisterSuccess");
                }
                catch (MemberDatabaseException e)
                {
                    log.Warn(e, "An error occurred whilst trying to register user: {0}", model.UserName);
                    ModelState.AddModelError("System", "An error occurred whilst attempting to register - please try again later");
                }
                catch (SendEmailException e)
                {
                    log.Warn(e, "Attempt to send email to {0} with email address {1} failed", model.UserName, model.Email);
                    return PartialView("RegisterSuccess");
                }
            }
            else if (!captchaValid)
            {
                ModelState.AddModelError("Recaptcha", "The reCAPTCHA wasn't entered correctly. Go back and try it again!");
            }

            return View(model);
        }

        /// <summary>
        /// GET recover password
        /// </summary>
        /// <returns></returns>
        public ActionResult RecoverPassword()
        {
            return View();
        }

        /// <summary>
        /// POST recover password
        /// </summary>
        /// <param name="model">Password model</param>
        /// <returns>Action</returns>
        [HttpPost]
        public ActionResult RecoverPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                log.Info("Received recover password request from {0}", model.UsernameOrEmail);

                try
                {
                    forgotPassword.ResetPassword(model.UsernameOrEmail);
                }
                catch (RecoverPasswordException e)
                {
                    ModelState.AddModelError("System", e.Message);
                    return View(model);
                }
            }

            return View("RecoverPasswordSuccess");
        }

        /// <summary>
        /// GET edit profile
        /// </summary>
        /// <returns>Action</returns>
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

        /// <summary>
        /// POST edit profile
        /// </summary>
        /// <param name="model">Edit Profile model</param>
        /// <returns>Action</returns>
        [HttpPost]
        public ActionResult EditProfile(EditProfile model)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return View("LogOn");
                }

                log.Debug("Updating profile for user: {0}", user.UserName);

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
                        log.Debug("Saving user: {0} profile picture with path: {1}", user.UserName, pathfile);
                        model.ProfilePicture.SaveAs(pathfile);
                        user.ProfilePicture = filename;
                    }
                    catch (Exception e)
                    {
                        log.Warn(e, "Could not save users: {0} profile picture with path: {1}", model.UserName, pathfile);
                    }

                    user.ProfilePicture = "~/uploads/avatars/" + user.ProfilePicture;
                }

                model.ProfilePicturePath = user.ProfilePicture;
                model.UserName = user.UserName;

                try
                {
                    memberDataAccess.UpdateUser(user);
                }
                catch (MemberDatabaseException e)
                {
                    log.Warn(e, "An error occurred whilst trying to register user: {0}", model.UserName);
                    ModelState.AddModelError("System", "An error occurred whilst attempting to update your profile - please try again later");
                    return View(model);
                }
            }

            return PartialView("EditProfileSuccess");
        }

        /// <summary>
        /// GET show profile
        /// </summary>
        /// <param name="userId">Userid to show</param>
        /// <returns>Action</returns>
        public ActionResult ShowProfile(string userId = null)
        {
            User userFromId = null;
            IList<Deal> deals = new List<Deal>();

            try
            {
                deals = dealDataAccess.GetAllDeals();

                if (userId != null)
                {
                    userFromId = memberDataAccess.GetUser(userId);
                }
            }
            catch (MemberDatabaseException)
            {
                log.Warn("Attempted to load profile for a non existant user {0}", userId);

                return this.PartialView("UserDoesNotExist");
            }
            catch (DealDatabaseException e)
            {
                log.Warn(e, "Could not load deals from the database");
            }

            if (user == null && userId == null)
            {
                return PartialView("LogOn");
            }

            User notNullUser = userFromId ?? user;

            List<Deal> userDeals = deals.Where(a => a.UserName.Equals(notNullUser.UserName, StringComparison.InvariantCultureIgnoreCase)).ToList();
            bool currentUser = notNullUser.UserName.Equals(user != null ? user.UserName : string.Empty, StringComparison.InvariantCultureIgnoreCase);

            log.Trace("Returning {0} deals for user {1} and current user is {2}", userDeals.Count, notNullUser.UserName, currentUser);

            UserDeals deal = new UserDeals
            {
                User = notNullUser,
                Deals = userDeals,
                IsCurrentUser = currentUser
            };

            return View(deal);
        }
    }
}
