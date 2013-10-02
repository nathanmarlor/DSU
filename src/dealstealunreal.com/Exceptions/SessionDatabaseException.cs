using System;

namespace dealstealunreal.com.Exceptions
{
    public class SessionDatabaseException : Exception
    {
        public SessionDatabaseException(string message) : base(message) { }
    }
}