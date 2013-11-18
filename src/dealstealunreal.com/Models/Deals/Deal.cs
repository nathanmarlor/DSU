namespace dealstealunreal.com.Models.Deals
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Deal
    {
        public int DealID { get; set; }

        public string UserName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Retailer { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Url { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DataType(DataType.ImageUrl)]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int Votes { get; set; }

        public bool Active { get; set; }

        public bool CanVote { get; set; }
    }
}