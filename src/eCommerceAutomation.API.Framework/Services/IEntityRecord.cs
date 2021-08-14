using System;
using eCommerceAutomation.Framework.Constants;

namespace eCommerceAutomation.API.Framework.Services
{
    public interface IEntityRecord
    {
        long Id
        {
            get;
            set;
        }

        Guid ViewId
        {
            get;
            set;
        }

        DateTime RecordInsertDateTime
        {
            get;
            set;
        }

        DateTime RecordUpdateDateTime
        {
            get;
            set;
        }

        RecordStatus RecordStatus
        {
            get;
            set;
        }
    }
}
