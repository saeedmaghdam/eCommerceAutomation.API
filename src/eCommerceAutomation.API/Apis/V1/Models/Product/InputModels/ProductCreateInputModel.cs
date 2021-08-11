using System.Collections.Generic;
using eCommerceAutomation.API.Apis.V1.Models.Source.InputModels;

namespace eCommerceAutomation.API.Apis.V1.Models.Product.InputModels
{
    public class ProductCreateInputModel
    {
        public string ExternalId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public IEnumerable<SourceInputModel> Sources
        {
            get;
            set;
        }
    }
}
