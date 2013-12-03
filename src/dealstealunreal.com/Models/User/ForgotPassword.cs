namespace dealstealunreal.com.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Forgot password
    /// </summary>
    public class ForgotPassword
    {
        /// <summary>
        /// Username or email
        /// </summary>
        [Required]
        [Display(Name = "Username or E-mail:")]
        public string UsernameOrEmail { get; set; }
    }
}