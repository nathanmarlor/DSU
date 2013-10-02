using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using dealstealunreal.com.Models.Deals;

namespace dealstealunreal.com.Models.Wrappers
{
    public class DealList
    {
        public IList<Deal> Deals { get; set; }

        public string CurrentUsername { get; set; }
    }
}