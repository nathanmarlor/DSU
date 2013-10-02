using dealstealunreal.com.Models.Deals;

namespace dealstealunreal.com.Models.Wrappers
{
    using System.Collections.Generic;
    using User;

    public class UserDeals
    {
        public User User { get; set; }

        public IList<Deal> Deals { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}