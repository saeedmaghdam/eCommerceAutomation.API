using eCommerceAutomation.Framework.Constants;
using eCommerceAutomation.API.Framework.Services.Product;

namespace eCommerceAutomation.API.Service.Product
{
    public class SourceNameModel : ISourceName
    {
        public SourceType SourceType
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }
    }
}
