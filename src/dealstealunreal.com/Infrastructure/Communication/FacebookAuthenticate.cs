namespace dealstealunreal.com.Infrastructure.Communication
{
    using System.IO;
    using System.Net;
    using Newtonsoft.Json;
    using Exceptions;
    using Interfaces;
    using Models.Facebook;

    public class FacebookAuthenticate : IFacebookAuthenticate
    {
        public FacebookResponse Authenticate(string userId, string token)
        {
            string url = string.Format("https://graph.facebook.com/{0}?method=GET&format=json&suppress_http_code=1&access_token={1}", userId, token);

            WebClient client = new WebClient();
            string response = null;
            using (Stream stream = client.OpenRead(url))
            {
                if (stream == null)
                {
                    // TODO: Add logging
                    throw new FacebookAuthenticationException();
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        response += line;
                    }
                }
            }

            return JsonConvert.DeserializeObject<FacebookResponse>(response);
        }
    }
}