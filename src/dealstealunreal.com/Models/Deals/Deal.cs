namespace dealstealunreal.com.Models.Deals
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Deal
    /// </summary>
    public class Deal
    {
        /// <summary>
        /// Deal ID
        /// </summary>
        public int DealID { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        /// <summary>
        /// Retailer
        /// </summary>
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Retailer { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        [Required]
        [DataType(DataType.Url)]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Url { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        /// <summary>
        /// Image
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Votes
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// Active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Can vote
        /// </summary>
        public bool CanVote { get; set; }
    }
}