namespace dealstealunreal.com.Data
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using dealstealunreal.com.Data.Interfaces;
    using dealstealunreal.com.Models;

    public class VoteDataAccess : IVoteDataAccess
    {
        private const string SaveVoteQuery = "insert into votes (DealId, Username, Date, Vote) values(@dealId, @userName, @date, @vote)";
        private const string GetVoteQuery = "select sum(Vote) as SumVotes from votes where DealId = @dealId";
        private const string CanVoteQuery = "select username from votes where Username = @username and DealId = @dealId";

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
            catch (SqlException e)
            {
                // TODO: Log!
            }
            catch (Exception e)
            {
                // TODO: Log!
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
            catch (SqlException e)
            {
                // TODO: Log!
            }
            catch (Exception e)
            {
                // TODO: Log!
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
            catch (SqlException e)
            {
                // TODO: Log!
            }
            catch (Exception e)
            {
                // TODO: Log!
            }

            return false;
        }
    }
}