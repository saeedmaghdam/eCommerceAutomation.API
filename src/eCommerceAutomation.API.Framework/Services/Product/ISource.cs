using eCommerceAutomation.API.Framework.Constants;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface ISource : IEntityRecord
    {
        int Priority
        {
            get;
            set;
        }

        SourceType SourceType
        {
            get;
            set;
        }

        string Address
        {
            get;
            set;
        }

        string OldMetadata
        {
            get;
            set;
        }

        string Metadata
        {
            get;
            set;
        }

        string PriceAdjustment
        {
            get;
            set;
        }

        string WholesalePriceAdjustment
        {
            get;
            set;
        }
    }
}
