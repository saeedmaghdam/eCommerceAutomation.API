using System.Collections.Generic;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface IProduct : IEntityRecord
    {
        string ExternalId
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

        string Url
        {
            get;
            set;
        }

        int? OriginalMinimumQuantity
        {
            get;
            set;
        }

        decimal? OriginalPrice
        {
            get;
            set;
        }

        string OriginalWholesalePrices
        {
            get;
            set;
        }

        int? MinimumQuantity
        {
            get;
            set;
        }

        decimal? Price
        {
            get;
            set;
        }

        string WholesalePrices
        {
            get;
            set;
        }

        bool IsReviewNeeded
        {
            get;
            set;
        }

        bool IsDisabled
        {
            get;
            set;
        }

        bool IsInitialized
        {
            get;
            set;
        }

        IEnumerable<ISource> Sources
        {
            get;
            set;
        }
    }
}
