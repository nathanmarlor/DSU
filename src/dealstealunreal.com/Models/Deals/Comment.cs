namespace dealstealunreal.com.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Comment
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Deal ID
        /// </summary>
        public int DealId { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string CommentString { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }
    }
}