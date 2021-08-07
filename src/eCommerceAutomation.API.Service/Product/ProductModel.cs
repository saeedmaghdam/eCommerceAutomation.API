using System.Collections.Generic;
using eCommerceAutomation.API.Framework.Services.Product;

namespace eCommerceAutomation.API.Service.Product
{
    public class ProductModel : EntityRecord, IProduct
    {
        public string ExternalId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public int? OriginalMinimumQuantity
        {
            get;
            set;
        }

        public decimal? OriginalPrice
        {
            get;
            set;
        }

        public string OriginalWholesalePrices
        {
            get;
            set;
        }

        public int? MinimumQuantity
        {
            get;
            set;
        }

        public decimal? Price
        {
            get;
            set;
        }

        public string WholesalePrices
        {
            get;
            set;
        }

        public bool IsReviewNeeded
        {
            get;
            set;
        }

        public bool IsDisabled
        {
            get;
            set;
        }

        public bool IsInitialized
        {
            get;
            set;
        }

        public IEnumerable<ISource> Sources
        {
            get;
            set;
        }
    }
}
