namespace dealstealunreal.com.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using Exceptions;
    using Interfaces;
    using Models;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Comment data access class
    /// </summary>
    public class CommentDataAccess : ICommentDataAccess
    {
        private const string GetDealCommentsQuery = "select Comments.Comment, Comments.Date, Comments.Username from Comments inner join Deals on Comments.DealId = Deals.DealId and Deals.DealId = @dealId";
        private const string SaveCommentQuery = "insert into Comments (Comment, Username, Date, DealId) values (@commentString, @userName, @date, @dealId)";
        private readonly ILogger log;

        /// <summary>
        /// Initialises a new instance of the <see cref="CommentDataAccess"/> class. 
        /// </summary>
        /// <param name="log">Logging module</param>
        public CommentDataAccess(ILogger log)
        {
            this.log = log;
        }

        /// <summary>
        /// Save deal comment
        /// </summary>
        /// <param name="comment">Comment to save</param>
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
            catch (Exception e)
            {
                log.Warn(e, "Could not save comment - {0} for user", comment.CommentString, comment.UserName);
            }
        }

        /// <summary>
        /// Get deal comments
        /// </summary>
        /// <param name="dealId">DealId</param>
        /// <returns>List of comments</returns>
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
            catch (Exception e)
            {
                log.Warn(e, "Could not get comments for deal {0}", dealId);
                throw new CommentDatabaseException();
            }
        }
    }
}