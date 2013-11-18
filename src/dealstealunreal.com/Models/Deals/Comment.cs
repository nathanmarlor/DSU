namespace dealstealunreal.com.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public int DealId { get; set; }

        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string CommentString { get; set; }

        public DateTime Date { get; set; }
    }
}