namespace dealstealunreal.com.Data.Interfaces
{
    using System;
    using Models;

    /// <summary>
    /// Interface for vote data access
    /// </summary>
    public interface IVoteDataAccess
    {
        /// <summary>
        /// Add vote
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <param name="userName">Username</param>
        /// <param name="date">Date</param>
        /// <param name="vote">Vote</param>
        void AddVote(int dealId, string userName, DateTime date, Vote vote);

        /// <summary>
        /// Get votes
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <returns>Number of votes</returns>
        int GetVotes(int dealId);

        /// <summary>
        /// Determine if a user can vote
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <param name="userName">Username</param>
        /// <returns>Can vote</returns>
        bool CanVote(int dealId, string userName);
    }
}