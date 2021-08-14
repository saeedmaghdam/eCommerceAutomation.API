using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Apis.V1.Models.Source.InputModels;
using eCommerceAutomation.API.Framework.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAutomation.API.Apis.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class SourceController : Controller
    {
        private readonly ISourceService _sourceService;

        public SourceController(ISourceService sourceService) => _sourceService = sourceService;

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> PatchSourceStatusAsync([FromRoute] long id, [FromBody] bool isDisabled, CancellationToken cancellationToken)
        {
            await _sourceService.PatchStatusAsync(id, isDisabled, cancellationToken);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAsync([FromRoute] long id, [FromBody] PatchSourceInputModel model, CancellationToken cancellationToken)
        {
            await _sourceService.PatchAsync(id, model.Priority, model.SourceType, model.Address, model.OldMetadata, model.Metadata, model.PriceAdjustment, model.WholesalePriceAdjustment, cancellationToken);

            return Ok();
        }
    }
}
