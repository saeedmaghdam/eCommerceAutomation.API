using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<IProduct>> GetAsync(bool? isReviewNeeded, bool? isDisabled, bool? isInitialized, bool? isSourcesDisabled, CancellationToken cancellationToken);

        Task<IProduct> CreateAsync(string externalId, string name, string url, IEnumerable<ServiceInputModel.SourceServiceInputModel> sources, CancellationToken cancellationToken);

        Task DeleteAsync(long id, CancellationToken cancellationToken);

        Task PutSourcesAsync(long id, IEnumerable<ServiceInputModel.SourceServiceInputModel> sources, CancellationToken cancellationToken);

        Task PatchStatusAsync(long id, bool isDisabled, CancellationToken cancellationToken);

        Task PatchReviewNeededStatusAsync(long id, bool isReviewNeeded, CancellationToken cancellationToken);

        Task PatchSourceStatusAsync(long sourceId, bool isDisabled, CancellationToken cancellationToken);
    }
}
