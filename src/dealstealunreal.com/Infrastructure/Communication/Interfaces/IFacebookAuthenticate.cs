namespace dealstealunreal.com.Infrastructure.Communication.Interfaces
{
    using Models.Facebook;

    public interface IFacebookAuthenticate
    {
        FacebookResponse Authenticate(string userId, string token);
    }
}