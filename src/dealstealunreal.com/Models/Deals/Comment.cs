namespace dealstealunreal.com.Models
{
    using System;

    public class Comment
    {
        public int DealId { get; set; }

        public string UserName { get; set; }

        public string CommentString { get; set; }

        public DateTime Date { get; set; }
    }
}