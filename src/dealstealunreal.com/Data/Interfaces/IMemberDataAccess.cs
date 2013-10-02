using dealstealunreal.com.Models;
using dealstealunreal.com.Models.User;

namespace dealstealunreal.com.Data.Interfaces
{
    public interface IMemberDataAccess
    {
        User GetUser(string userId);

        void CreateUser(Register details);

        void ChangePassword(string userId, string password);

        void UpdateUser(User user);
    }
}