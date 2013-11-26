namespace dealstealunreal.com.Infrastructure.Utilities
{
    using Models.User;

    public interface ICurrentUser
    {
        User GetCurrentUser();
    }
}