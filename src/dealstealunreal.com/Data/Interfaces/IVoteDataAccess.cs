namespace dealstealunreal.com.Data.Interfaces
{
    using System;
    using dealstealunreal.com.Models;

    public interface IVoteDataAccess
    {
        void AddVote(int dealId, string userName, DateTime date, Vote vote);

        int GetVotes(int dealId);

        bool CanVote(int dealId, string userName);
    }
}