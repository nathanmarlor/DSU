namespace dealstealunreal.com.Infrastructure.Processing.Interfaces
{
    /// <summary>
    /// Vote processing
    /// </summary>
    public interface IVoteProcessor
    {
        /// <summary>
        /// Calculate vote weighting
        /// </summary>
        /// <param name="votes">Deal ID</param>
        /// <returns>Weight</returns>
        int CalculateVote(int votes);
    }
}