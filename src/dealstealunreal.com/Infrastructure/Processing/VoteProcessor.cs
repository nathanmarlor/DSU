using dealstealunreal.com.Models.Deals;

namespace dealstealunreal.com.Infrastructure.Processing
{
    using System.Configuration;
    using Data.Interfaces;
    using Interfaces;

    public class VoteProcessor : IVoteProcessor
    {
        private readonly IDealDataAccess dealDataAccess;

        public VoteProcessor(IDealDataAccess dealDataAccess)
        {
            this.dealDataAccess = dealDataAccess;
        }

        public int CalculateVote(int dealId)
        {
            double dealLimit = double.Parse(ConfigurationManager.AppSettings["Deal"]);
            double stealLimit = double.Parse(ConfigurationManager.AppSettings["Steal"]);
            double unrealLimit = double.Parse(ConfigurationManager.AppSettings["Unreal"]);

            Deal deal = dealDataAccess.GetDeal(dealId);

            int numOfVotes = 0;

            if (deal.Votes > 0 && deal.Votes <= dealLimit)
            {
                numOfVotes = (int)(deal.Votes * dealLimit);
            }

            if (deal.Votes > dealLimit && deal.Votes <= stealLimit)
            {
                numOfVotes = (int)((deal.Votes - (dealLimit + stealLimit)) / unrealLimit);
            }

            if (deal.Votes > stealLimit)
            {
                numOfVotes = (int)((deal.Votes - dealLimit) / stealLimit);
            }

            return numOfVotes;
        }
    }
}