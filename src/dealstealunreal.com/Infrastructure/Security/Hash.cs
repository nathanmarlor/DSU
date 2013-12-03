namespace dealstealunreal.com.Infrastructure.Security
{
    using System;
    using Interfaces;

    /// <summary>
    /// Hashing class
    /// </summary>
    public class Hash : IHash
    {
        /// <summary>
        /// Hash a string
        /// </summary>
        /// <param name="input">String to hash</param>
        /// <returns>Hashed string</returns>
        public string HashString(string input)
        {
            var data = System.Text.Encoding.ASCII.GetBytes(input);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}