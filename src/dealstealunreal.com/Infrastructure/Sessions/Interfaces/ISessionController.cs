namespace dealstealunreal.com.Infrastructure.Sessions.Interfaces
{

    using dealstealunreal.com.Models.Sessions;

    public interface ISessionController
    {
        bool Logon(string username, string password, bool rememberMe);

        void Logoff();

        Session GetCurrentUser();

        void PruneSessions();
    }
}