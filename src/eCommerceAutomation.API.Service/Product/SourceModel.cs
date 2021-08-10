using eCommerceAutomation.API.Framework.Constants;
using eCommerceAutomation.API.Framework.Services.Product;

namespace eCommerceAutomation.API.Service.Product
{
    public class SourceModel : EntityRecord, ISource
    {
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

        public string OldMetadata
        {
            get;
            set;
        }

        public string Metadata
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

        public bool IsDisabled
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }
    }
}
