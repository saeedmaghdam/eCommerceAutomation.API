namespace eCommerceAutomation.API.Apis.V1.Models.Product.InputModels
{
    public class PostUpdateWebsiteSource
    {
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
    }
}
