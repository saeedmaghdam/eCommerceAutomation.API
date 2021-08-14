using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.Framework.Constants;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface ISourceService
    {
        Task PatchStatusAsync(long id, bool isDisabled, CancellationToken cancellationToken);

        Task PatchAsync(long id, int? priority, SourceType? sourceType, string address, string oldMetadata, string metadata, string priceAdjustment, string wholesalePriceAdjustment, CancellationToken cancellationToken);

        Task ActiveSourceAsync(long id, CancellationToken cancellationToken);
    }
}
