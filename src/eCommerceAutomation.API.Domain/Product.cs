using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceAutomation.API.Domain
{
    [Table("product", Schema = "product")]
    public class Product : Entity
    {
        [Column("external_id")]
        public string ExternalId
        {
            get;
            set;
        }

        [Column("name")]
        public string Name
        {
            get;
            set;
        }

        [Column("url")]
        public string Url
        {
            get;
            set;
        }

        [Column("original_minimum_quantity")]
        public int? OriginalMinimumQuantity
        {
            get;
            set;
        }

        [Column("original_price")]
        public decimal? OriginalPrice
        {
            get;
            set;
        }

        [Column("original_wholesale_prices")]
        public string OriginalWholesalePrices
        {
            get;
            set;
        }

        [Column("minimum_quantity")]
        public int? MinimumQuantity
        {
            get;
            set;
        }

        [Column("price")]
        public decimal? Price
        {
            get;
            set;
        }

        [Column("wholesale_prices")]
        public string WholesalePrices
        {
            get;
            set;
        }

        [Column("is_review_needed")]
        public bool IsReviewNeeded
        {
            get;
            set;
        }

        [Column("is_disabled")]
        public bool IsDisabled
        {
            get;
            set;
        }

        [Column("is_initialized")]
        public bool IsInitialized
        {
            get;
            set;
        }

        public virtual ICollection<Source> Sources
        {
            get;
            set;
        }

        public static Product Create()
        {
            var product = new Product();
            product.ViewId = System.Guid.NewGuid();
            product.RecordStatus = Framework.Constants.RecordStatus.Inserted;
            product.RecordInsertDateTime = System.DateTime.Now;
            product.RecordUpdateDateTime = System.DateTime.Now;
            product.IsDisabled = false;
            product.IsInitialized = false;
            product.IsReviewNeeded = false;

            return product;
        }
    }
}
