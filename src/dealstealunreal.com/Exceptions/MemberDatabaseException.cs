using System;

namespace dealstealunreal.com.Exceptions
{
    public class MemberDatabaseException : Exception
    {
        public MemberDatabaseException() { }

        public MemberDatabaseException(string message) : base(message) { }
    }
}