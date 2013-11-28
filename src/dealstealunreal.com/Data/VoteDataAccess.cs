namespace dealstealunreal.com.Data
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using Interfaces;
    using Models;
    using Ninject.Extensions.Logging;

    public class VoteDataAccess : IVoteDataAccess
    {
        private const string SaveVoteQuery = "insert into votes (DealId, Username, Date, Vote) values(@dealId, @userName, @date, @vote)";
        private const string GetVoteQuery = "select COALESCE(sum(Vote), 0) as SumVotes from votes where DealId = 6";
        private const string CanVoteQuery = "select username from votes where Username = @username and DealId = @dealId";
        private readonly ILogger log;

        public VoteDataAccess(ILogger log)
        {
            this.log = log;
        }

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