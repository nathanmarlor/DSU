namespace dealstealunreal.com.Models.Wrappers
{
    using System.Collections.Generic;

    using dealstealunreal.com.Models.Deals;

    public class DealComments
    {
        public Deal Deal { get; set; }

        public IDictionary<User.User, Comment> Comments { get; set; }

        public string CurrentUsername { get; set; }

        public string NewComment { get; set; }
    }
}