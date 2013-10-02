namespace dealstealunreal.com.Infrastructure.Security
{
    using Interfaces;
    using System;

    public class Hash : IHash
    {
        public string HashString(string input)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(input);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}