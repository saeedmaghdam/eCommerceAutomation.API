using eCommerceAutomation.API.Framework.Constants;

namespace eCommerceAutomation.API.Framework.Services.Product.ServiceInputModel
{
    public class SourceServiceInputModel
    {
        public long? Id
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public SourceType SourceType
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public string PriceAdjustment
        {
            get;
            set;
        }

        public string WholesalePriceAdjustment
        {
            get;
            set;
        }
    }
}
