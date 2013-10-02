using System;

namespace dealstealunreal.com.Exceptions
{
    public class RecoverPasswordException : Exception
    {
        public RecoverPasswordException(string message)
            : base(message)
        {

        }
    }
}