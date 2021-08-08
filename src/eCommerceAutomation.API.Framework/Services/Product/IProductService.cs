using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface IProductService
    {
        Task<IProduct> CreateAsync(string externalId, string name, string url, IEnumerable<ServiceInputModel.SourceServiceInputModel> sources, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);

        Task PutSourcesAsync(long id, IEnumerable<ServiceInputModel.SourceServiceInputModel> sources, CancellationToken cancellationToken);
    }
}
