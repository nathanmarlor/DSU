namespace dealstealunreal.com.Data.Interfaces
{
    using System.Collections.Generic;
    using Models.Deals;

    /// <summary>
    /// Interface for deal data access
    /// </summary>
    public interface IDealDataAccess
    {
        /// <summary>
        /// Get all deals
        /// </summary>
        /// <returns>List of deals</returns>
        IList<Deal> GetAllDeals();

        /// <summary>
        /// Get specific deal
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <returns>Deal</returns>
        Deal GetDeal(int dealId);

        /// <summary>
        /// Search for deal
        /// </summary>
        /// <param name="dealName">Deal name</param>
        /// <returns>List of deals</returns>
        IList<Deal> SearchForDeal(string dealName);

        /// <summary>
        /// Save deal
        /// </summary>
        /// <param name="deal">Deal model</param>
        void SaveDeal(Deal deal);

        /// <summary>
        /// Save deal description
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        /// <param name="description">Description</param>
        void SaveDealDescription(int dealId, string description);

        /// <summary>
        /// Save deal as active/inactive
        /// </summary>
        /// <param name="dealId">DealId</param>
        /// <param name="active">Active/Inactive</param>
        void SaveDealActive(int dealId, bool active);

        /// <summary>
        /// Delete deal
        /// </summary>
        /// <param name="dealId">Deal Id</param>
        void DeleteDeal(int dealId);
    }
}