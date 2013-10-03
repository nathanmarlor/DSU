namespace dealstealunreal.com.Models.Wrappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Deals;

    public class DealComments
    {
        public Deal Deal { get; set; }

        public IOrderedEnumerable<KeyValuePair<User.User, Comment>> Comments { get; set; }

        public string CurrentUsername { get; set; }

        public string NewComment { get; set; }
    }
}