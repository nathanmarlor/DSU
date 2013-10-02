namespace dealstealunreal.com.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPassword
    {
        [Required]
        [Display(Name = "Username or E-mail:")]
        public string UsernameOrEmail { get; set; }
    }
}