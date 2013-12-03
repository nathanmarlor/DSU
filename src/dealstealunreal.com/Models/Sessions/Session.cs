namespace dealstealunreal.com.Models.Sessions
{
    using System;

    /// <summary>
    /// Session
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Session ID
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Remember me
        /// </summary>
        public bool RememberMe { get; set; }
    }
}