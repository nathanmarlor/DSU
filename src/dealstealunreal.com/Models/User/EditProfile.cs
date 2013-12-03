namespace dealstealunreal.com.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using DataAnnotationsExtensions;

    /// <summary>
    /// Edit profile
    /// </summary>
    public class EditProfile
    {
        /// <summary>
        /// Username
        /// </summary>
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [Email]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        /// <summary>
        /// Picture
        /// </summary>
        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        /// <summary>
        /// Confirm password
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Profile picture
        /// </summary>
        public string ProfilePicturePath { get; set; }
    }
}