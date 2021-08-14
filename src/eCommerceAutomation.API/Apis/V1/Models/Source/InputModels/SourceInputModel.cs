using eCommerceAutomation.Framework.Constants;

namespace eCommerceAutomation.API.Apis.V1.Models.Source.InputModels
{
    public class SourceInputModel
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

        public string Key
        {
            get;
            set;
        }
    }
}
