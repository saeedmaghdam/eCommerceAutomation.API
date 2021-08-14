using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Apis.V1.Models.SourceName.ViewModels;
using eCommerceAutomation.Framework.Constants;
using eCommerceAutomation.API.Framework.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAutomation.API.Apis.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SourceNameController : Controller
    {
        private readonly ISourceNameService _sourceNameService;

        public SourceNameController(ISourceNameService sourceNameService)
        {
            _sourceNameService = sourceNameService;
        }

        [HttpGet]
        public async Task<ActionResult<SourceNameViewModel>> GetAsync(SourceType? sourceType, CancellationToken cancellationToken)
        {
            var sourceNames = await _sourceNameService.GetAsync(sourceType, cancellationToken);

            return Ok(sourceNames.Select(sourceName => new SourceNameViewModel()
            {
                SourceType = sourceName.SourceType,
                Key = sourceName.Key
            }));
        }
    }
}
