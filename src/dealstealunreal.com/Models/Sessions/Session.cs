namespace dealstealunreal.com.Models.Sessions
{
    using System;
    using User;

    public class Session
    {
        public Guid SessionId { get; set; }

        public string Username { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool RememberMe { get; set; }
    }
}