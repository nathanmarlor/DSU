namespace dealstealunreal.com.Data
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using Interfaces;
    using Models;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Vote data access
    /// </summary>
    public class VoteDataAccess : IVoteDataAccess
    {
        private const string SaveVoteQuery = "insert into votes (DealId, Username, Date, Vote) values(@dealId, @userName, @date, @vote)";
        private const string GetVoteQuery = "select COALESCE(sum(Vote), 0) as SumVotes from votes where DealId = 6";
        private const string CanVoteQuery = "select username from votes where Username = @username and DealId = @dealId";
        private readonly ILogger log;

        /// <summary>
        /// Initialises a new instance of the <see cref="VoteDataAccess"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        public VoteDataAccess(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Add vote
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <param name="userName">Username</param>
        /// <param name="date">Date</param>
        /// <param name="vote">Vote</param>
        public void AddVote(int dealId, string userName, DateTime date, Vote vote)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = SaveVoteQuery;

                        command.Parameters.AddWithValue("@dealId", dealId);
                        command.Parameters.AddWithValue("@vote", vote);
                        command.Parameters.AddWithValue("@userName", userName);
                        command.Parameters.AddWithValue("@date", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not add vote to deal: {0} user: {1} vote: {2}", dealId, userName, vote);
            }
        }

        /// <summary>
        /// Get votes
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <returns>Number of votes</returns>
        public int GetVotes(int dealId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = GetVoteQuery;

                        command.Parameters.AddWithValue("@dealId", dealId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader.GetInt32(reader.GetOrdinal("SumVotes"));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not get votes for deal: {0}", dealId);
            }

            return 0;
        }

        /// <summary>
        /// Determine if a user can vote
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <param name="userName">Username</param>
        /// <returns>Can vote</returns>
        public bool CanVote(int dealId, string userName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = CanVoteQuery;

                        command.Parameters.AddWithValue("@dealId", dealId);
                        command.Parameters.AddWithValue("@userName", userName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            return !reader.Read();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Warn(e, "Could not determine if a user can vote on deal: {0} user: {1}", dealId, userName);
            }

            return false;
        }
    }
}