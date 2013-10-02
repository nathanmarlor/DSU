namespace dealstealunreal.com.Data.Interfaces
{
    using System.Collections.Generic;
    using dealstealunreal.com.Models;

    public interface ICommentDataAccess
    {
        void SaveDealComment(Comment comment);

        IList<Comment> GetDealComments(int dealId);
    }
}