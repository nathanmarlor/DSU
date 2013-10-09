namespace dealstealunreal.com.Models.Deals
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Deal
    {
        public int DealID { get; set; }

        public string UserName { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Retailer { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public DateTime Date { get; set; }

        public int Votes { get; set; }

        public bool Active { get; set; }

        public bool CanVote { get; set; }
    }
}