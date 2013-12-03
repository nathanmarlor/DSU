namespace dealstealunreal.com.Models.Wrappers
{
    using System.Linq;
    using Deals;

    /// <summary>
    /// Ordered deals
    /// </summary>
    public class OrderedDeals
    {
        /// <summary>
        /// Deals
        /// </summary>
        public IOrderedEnumerable<Deal> Deals { get; set; }

        /// <summary>
        /// Current user
        /// </summary>
        public string CurrentUsername { get; set; }
    }
}