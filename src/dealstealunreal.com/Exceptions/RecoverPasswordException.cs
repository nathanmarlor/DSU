namespace dealstealunreal.com.Exceptions
{
    using System;

    public class RecoverPasswordException : Exception
    {

        public RecoverPasswordException()
        {

        }

        public RecoverPasswordException(string message)
            : base(message)
        {

        }
    }
}