using System.ComponentModel.DataAnnotations.Schema;
using eCommerceAutomation.API.Framework.Constants;

namespace eCommerceAutomation.API.Domain
{
    [Table("source", Schema = "product")]
    public class Source : Entity
    {
        [ForeignKey("ProductId")]
        public Product Product
        {
            get;
            set;
        }

        [Column("product_id")]
        public long ProductId
        {
            get;
            set;
        }

        [Column("priority")]
        public int Priority
        {
            get;
            set;
        }

        [Column("source_type")]
        public SourceType SourceType
        {
            get;
            set;
        }

        [Column("address")]
        public string Address
        {
            get;
            set;
        }

        [Column("old_metadata")]
        public string OldMetadata
        {
            get;
            set;
        }

        [Column("metadata")]
        public string Metadata
        {
            get;
            set;
        }

        [Column("price_adjustment")]
        public string PriceAdjustment
        {
            get;
            set;
        }

        [Column("wholesale_price_adjustment")]
        public string WholesalePriceAdjustment
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

        [Column("key")]
        public string Key
        {
            get;
            set;
        }

        public static Source Create()
        {
            var source = new Source();
            source.ViewId = System.Guid.NewGuid();
            source.RecordStatus = Framework.Constants.RecordStatus.Inserted;
            source.RecordInsertDateTime = System.DateTime.Now;
            source.RecordUpdateDateTime = System.DateTime.Now;

            return source;
        }
    }
}
