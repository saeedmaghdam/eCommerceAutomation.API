using System.Threading;
using System.Threading.Tasks;

namespace eCommerceAutomation.API.Framework.Services.Product
{
    public interface ISourceService
    {
        Task PatchStatusAsync(long id, bool isDisabled, CancellationToken cancellationToken);
    }
}
