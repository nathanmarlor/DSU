namespace dealstealunreal.com.Models.User
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Profile picture
        /// </summary>
        public string ProfilePicture { get; set; }

        /// <summary>
        /// Points
        /// </summary>
        public int Points { get; set; }
    }
}