using System.Collections.Generic;

namespace eCommerceAutomation.API.Apis.V1.Models.Source.InputModels
{
    public class PutSourcesInputModel
    {
        public IEnumerable<SourceInputModel> Sources
        {
            get;
            set;
        }
    }
}
