using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Apis.V1.Models.Source.InputModels;
using eCommerceAutomation.API.Framework.Services.Product;
using eCommerceAutomation.API.Framework.Services.Product.ServiceInputModel;
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
    }
}
