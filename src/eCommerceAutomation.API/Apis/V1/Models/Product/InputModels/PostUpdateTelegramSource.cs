namespace eCommerceAutomation.API.Apis.V1.Models.Product.InputModels
{
    public class PostUpdateTelegramSource
    {
        public string PriceAdjustment
        {
            get;
            set;
        }

        public decimal Price
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
