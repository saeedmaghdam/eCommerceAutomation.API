using System;
using eCommerceAutomation.API.Framework.Constants;

namespace eCommerceAutomation.API.Apis.V1.Models.Product.ViewModels
{
    public class SourceViewModel
    {
        public long Id
        {
            get;
            set;
        }

        public Guid ViewId
        {
            get;
            set;
        }

        public DateTime RecordInsertDateTime
        {
            get;
            set;
        }

        public DateTime RecordUpdateDateTime
        {
            get;
            set;
        }

        public RecordStatus RecordStatus
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
    }
}
