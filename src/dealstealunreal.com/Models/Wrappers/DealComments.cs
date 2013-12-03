namespace dealstealunreal.com.Models.Wrappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Deals;

    /// <summary>
    /// Deal with comments
    /// </summary>
    public class DealComments
    {
        /// <summary>
        /// Deal
        /// </summary>
        public Deal Deal { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public IOrderedEnumerable<KeyValuePair<User.User, Comment>> Comments { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string CurrentUsername { get; set; }

        /// <summary>
        /// New comment
        /// </summary>
        public string NewComment { get; set; }
    }
}