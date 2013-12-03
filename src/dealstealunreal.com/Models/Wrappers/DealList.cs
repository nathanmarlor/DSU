namespace dealstealunreal.com.Models.Wrappers
{
    using System.Collections.Generic;
    using Deals;

    /// <summary>
    /// Deal list
    /// </summary>
    public class DealList
    {
        /// <summary>
        /// Deals
        /// </summary>
        public IList<Deal> Deals { get; set; }

        /// <summary>
        /// Current user
        /// </summary>
        public string CurrentUsername { get; set; }
    }
}