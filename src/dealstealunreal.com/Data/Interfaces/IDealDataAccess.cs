namespace dealstealunreal.com.Data.Interfaces
{
    using System.Collections.Generic;
    using dealstealunreal.com.Models.Deals;

    public interface IDealDataAccess
    {
        IList<Deal> GetAllDeals();

        Deal GetDeal(int dealId);

        IList<Deal> SearchForDeal(string dealName);

        void SaveDeal(Deal deal);

        void SaveDealDescription(int dealId, string description);

        void SaveDealActive(int dealId, bool active);

        void DeleteDeal(int dealId);
    }
}