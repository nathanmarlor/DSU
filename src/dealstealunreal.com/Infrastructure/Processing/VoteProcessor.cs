namespace dealstealunreal.com.Infrastructure.Processing
{
    using System.Configuration;
    using Interfaces;

    public class VoteProcessor : IVoteProcessor
    {
        public int CalculateVote(int votes)
        {
            double dealLimit = double.Parse(ConfigurationManager.AppSettings["Deal"]);
            double stealLimit = double.Parse(ConfigurationManager.AppSettings["Steal"]);
            double unrealLimit = double.Parse(ConfigurationManager.AppSettings["Unreal"]);

            if (votes <= 0)
            {
                return 0;
            }

            if (votes > 0 && votes <= dealLimit)
            {
                return (int)((votes / dealLimit) * (100 / 3));
            }

            if (votes > dealLimit && votes <= stealLimit)
            {
                return (int)(((votes - dealLimit) / (stealLimit - dealLimit)) * (100 / 3)) + (100 / 3);
            }

            if (votes > stealLimit && votes <= unrealLimit)
            {
                return (int)(((votes - stealLimit) / (unrealLimit - stealLimit)) * (100 / 3)) + (200 / 3);
            }

            return 100;
        }
    }
}