namespace dealstealunreal.com.Models.Wrappers
{
    using System.Linq;
    using dealstealunreal.com.Models.Deals;

    public class OrderedDeals
    {
        public IOrderedEnumerable<Deal> Deals { get; set; }

        public string CurrentUsername { get; set; }
    }
}