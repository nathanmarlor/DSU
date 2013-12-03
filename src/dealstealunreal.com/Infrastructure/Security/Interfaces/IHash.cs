namespace dealstealunreal.com.Infrastructure.Security.Interfaces
{
    /// <summary>
    /// Hashing interface
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// Hash a string
        /// </summary>
        /// <param name="input">String to hash</param>
        /// <returns>Hashed string</returns>
        string HashString(string input);
    }
}