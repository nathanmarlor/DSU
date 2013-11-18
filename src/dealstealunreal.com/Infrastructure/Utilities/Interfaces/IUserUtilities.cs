namespace dealstealunreal.com.Infrastructure.Utilities.Interfaces
{
    using Models.User;

    public interface IUserUtilities
    {
        User GetCurrentUser();
    }
}