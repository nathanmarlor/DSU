using System.Linq;

namespace dealstealunreal.com.Models.Wrappers
{
    using System.Collections.Generic;
    using Deals;
    using User;

    /// <summary>
    /// User deals
    /// </summary>
    public class UserDeals
    {
        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Deals
        /// </summary>
        public IOrderedEnumerable<Deal> Deals { get; set; }

        /// <summary>
        /// Is current user
        /// </summary>
        public bool IsCurrentUser { get; set; }
    }
}