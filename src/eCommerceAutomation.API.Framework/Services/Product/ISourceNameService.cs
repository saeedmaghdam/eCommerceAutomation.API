using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Framework.Constants;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface ISourceNameService
    {
        Task<IEnumerable<ISourceName>> GetAsync(SourceType? sourceType, CancellationToken cancellationToken);
    }
}
