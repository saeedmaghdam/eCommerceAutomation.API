using System;
using eCommerceAutomation.API.Framework.Constants;

namespace eCommerceAutomation.API.Service
{
    public class EntityRecord
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
    }
}
