namespace dealstealunreal.com.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using dealstealunreal.com.Data.Interfaces;
    using dealstealunreal.com.Exceptions;
    using dealstealunreal.com.Models;

    public class CommentDataAccess : ICommentDataAccess
    {
        private const string GetDealCommentsQuery = "select Comments.Comment, Comments.Date, Comments.Username from Comments inner join Deals on Comments.DealId = Deals.DealId and Deals.DealId = @dealId";
        private const string SaveCommentQuery = "insert into Comments (Comment, Username, Date, DealId) values (@commentString, @userName, @date, @dealId)";

        public void SaveDealComment(Comment comment)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadWriteDatabase"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = SaveCommentQuery;

                        command.Parameters.AddWithValue("@dealId", comment.DealId);
                        command.Parameters.AddWithValue("@commentString", comment.CommentString);
                        command.Parameters.AddWithValue("@userName", comment.UserName);
                        command.Parameters.AddWithValue("@date", comment.Date);

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

        public IList<Comment> GetDealComments(int dealId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ReadonlyDatabase"].ConnectionString;

            IList<Comment> comments = new List<Comment>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = GetDealCommentsQuery;
                        command.Parameters.AddWithValue("@dealId", dealId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comments.Add(
                                    new Comment
                                        {
                                            UserName = reader.GetString(reader.GetOrdinal("Username")).Trim(),
                                            CommentString = reader.GetString(reader.GetOrdinal("Comment")).Trim(),
                                            Date = reader.GetDateTime(reader.GetOrdinal("Date"))
                                        });
                            }
                        }
                    }
                }

                return comments;
            }
            catch (SqlException e)
            {
                // TODO: Log!
                throw new CommentDatabaseException();
            }
            catch (Exception e)
            {
                // TODO: Log!
                throw new CommentDatabaseException();
            }
        }
    }
}