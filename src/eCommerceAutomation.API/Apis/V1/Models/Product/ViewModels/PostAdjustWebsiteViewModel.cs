using System;
using System.Collections.Generic;

namespace eCommerceAutomation.API.Apis.V1.Models.Product.ViewModels
{
    public class PostAdjustWebsiteViewModel
    {
        public decimal Price
        {
            get;
            set;
        }

        public List<Tuple<int, decimal>> WholesalePrices
        {
            get;
            set;
        }
    }
}
