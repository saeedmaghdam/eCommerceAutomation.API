namespace eCommerceAutomation.API.Apis.V1.Models.Product.InputModels
{
    public class PatchProductInputModel
    {
        public string Name
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

        public bool? IsReviewNeeded
        {
            get;
            set;
        }

        public bool? IsInitialized
        {
            get;
            set;
        }
    }
}
