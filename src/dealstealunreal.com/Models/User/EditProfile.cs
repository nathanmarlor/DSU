namespace dealstealunreal.com.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using DataAnnotationsExtensions;

    public class EditProfile
    {
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [Email]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}