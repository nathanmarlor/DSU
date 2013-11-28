namespace dealstealunreal.com.Data.Interfaces
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Interface for comment data access
    /// </summary>
    public interface ICommentDataAccess
    {
        /// <summary>
        /// Save deal comment
        /// </summary>
        /// <param name="comment">Comment to save</param>
        void SaveDealComment(Comment comment);

        /// <summary>
        /// Get deal comments
        /// </summary>
        /// <param name="dealId">DealId</param>
        /// <returns>List of comments</returns>
        IList<Comment> GetDealComments(int dealId);
    }
}