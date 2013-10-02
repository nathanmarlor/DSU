namespace dealstealunreal.com.Models
{
    public class FbLogin
    {
        public string AccessToken { get; set; }

        public string ExpiresIn { get; set; }

        public string SignedRequest { get; set; }

        public string UserId { get; set; }
    }
}