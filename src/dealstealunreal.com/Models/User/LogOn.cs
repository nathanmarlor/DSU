namespace dealstealunreal.com.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Log on
    /// </summary>
    public class LogOn
    {
        /// <summary>
        /// Username
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Remember me
        /// </summary>
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}