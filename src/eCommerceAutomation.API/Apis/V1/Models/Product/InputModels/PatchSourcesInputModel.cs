using System.Collections.Generic;

namespace eCommerceAutomation.API.Apis.V1.Models.Product.InputModels
{
    public class PatchSourcesInputModel
    {
        public IEnumerable<SourceInputModel> Sources
        {
            get;
            set;
        }
    }
}
