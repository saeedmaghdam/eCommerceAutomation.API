using eCommerceAutomation.Framework.Constants;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface ISourceName
    {
        SourceType SourceType
        {
            get;
            set;
        }

        string Key
        {
            get;
            set;
        }
    }
}
