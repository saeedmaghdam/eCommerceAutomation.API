namespace eCommerceAutomation.API.Apis.V1.Models.Product.InputModels
{
    public class PostAdjustTelegramInputModel
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
    }
}
