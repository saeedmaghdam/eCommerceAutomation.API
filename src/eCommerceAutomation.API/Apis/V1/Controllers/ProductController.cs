using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Apis.V1.Models.Product.InputModels;
using eCommerceAutomation.API.Apis.V1.Models.Product.ViewModels;
using eCommerceAutomation.API.Apis.V1.Models.Source.InputModels;
using eCommerceAutomation.API.Apis.V1.Models.Source.ViewModels;
using eCommerceAutomation.API.Framework.Services.Product;
using eCommerceAutomation.API.Framework.Services.Product.ServiceInputModel;
using eCommerceAutomation.Framework;
using eCommerceAutomation.Framework.Constants;
using eCommerceAutomation.Framework.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommerceAutomation.API.Apis.V1.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly CommonHelper _commonHelper;

        public ProductController(IProductService productService, CommonHelper commonHelper)
        {
            _productService = productService;
            _commonHelper = commonHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAsync(bool? isReviewNeeded, bool? isDisabled, bool? isInitialized, bool? isSourcesDisabled, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAsync(isReviewNeeded, isDisabled, isInitialized, isSourcesDisabled, cancellationToken);
            if (!products.Any())
                return NoContent();

            return Ok(products.Select(product => new ProductViewModel()
            {
                Sources = product.Sources.Select(source => new SourceViewModel()
                {
                    ViewId = source.ViewId,
                    RecordUpdateDateTime = source.RecordUpdateDateTime,
                    RecordStatus = source.RecordStatus,
                    RecordInsertDateTime = source.RecordInsertDateTime,
                    Address = source.Address,
                    Id = source.Id,
                    IsDisabled = source.IsDisabled,
                    Metadata = source.Metadata,
                    OldMetadata = source.OldMetadata,
                    PriceAdjustment = source.PriceAdjustment,
                    Priority = source.Priority,
                    SourceType = source.SourceType,
                    WholesalePriceAdjustment = source.WholesalePriceAdjustment,
                    Key = source.Key,
                    IsActive = source.IsActive

                }),
                ExternalId = product.ExternalId,
                Id = product.Id,
                IsDisabled = product.IsDisabled,
                IsInitialized = product.IsInitialized,
                IsReviewNeeded = product.IsReviewNeeded,
                MinimumQuantity = product.MinimumQuantity,
                Name = product.Name,
                OriginalMinimumQuantity = product.OriginalMinimumQuantity,
                OriginalPrice = product.OriginalPrice,
                OriginalWholesalePrices = product.OriginalWholesalePrices,
                Price = product.Price,
                RecordInsertDateTime = product.RecordInsertDateTime,
                RecordStatus = product.RecordStatus,
                RecordUpdateDateTime = product.RecordUpdateDateTime,
                Url = product.Url,
                ViewId = product.ViewId,
                WholesalePrices = product.WholesalePrices
            }));
        }

        [HttpPost("createProductWithSources")]
        public async Task<ActionResult<long>> CreateAsync([FromBody] ProductCreateInputModel model, CancellationToken cancellationToken)
        {
            var product = await _productService.CreateWithSourcesAsync(model.ExternalId, model.Name, model.Url, model.Sources.Select(source => new SourceServiceInputModel()
            {
                SourceType = source.SourceType,
                Address = source.Address,
                PriceAdjustment = source.PriceAdjustment,
                Priority = source.Priority,
                WholesalePriceAdjustment = source.WholesalePriceAdjustment,
                Key = source.Key
            }), cancellationToken);

            return product.Id;
        }

        [HttpPut("{id}/Sources")]
        public async Task<ActionResult> PutSourcesAsync([FromRoute] long id, [FromBody] PutSourcesInputModel model, CancellationToken cancellationToken)
        {
            await _productService.PutSourcesAsync(id, model.Sources.Select(source => new SourceServiceInputModel()
            {
                Id = source.Id,
                SourceType = source.SourceType,
                Address = source.Address,
                PriceAdjustment = source.PriceAdjustment,
                Priority = source.Priority,
                WholesalePriceAdjustment = source.WholesalePriceAdjustment,
                Key = source.Key
            }), cancellationToken);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);

            return Ok();
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> PatchStatusAsync([FromRoute] long id, [FromBody] bool isDisabled, CancellationToken cancellationToken)
        {
            await _productService.PatchStatusAsync(id, isDisabled, cancellationToken);

            return Ok();
        }

        [HttpPatch("{id}/reviewNeededStatus")]
        public async Task<ActionResult> PatchReviewNeededStatusAsync([FromRoute] long id, [FromBody] bool isReviewNeeded, CancellationToken cancellationToken)
        {
            await _productService.PatchReviewNeededStatusAsync(id, isReviewNeeded, cancellationToken);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchAsync([FromRoute] long id, [FromBody] PatchProductInputModel model, CancellationToken cancellationToken)
        {
            await _productService.PatchProductAsync(id, model.Name, model.OriginalMinimumQuantity, model.OriginalPrice, model.OriginalWholesalePrices, model.MinimumQuantity, model.Price, model.WholesalePrices, model.IsReviewNeeded, model.IsInitialized, cancellationToken);

            return Ok();
        }

        [HttpPost("{id}/source/{sourceId}/adjustWebsiteModel")]
        public async Task<ActionResult<PostAdjustWebsiteViewModel>> PostAdjustWebsiteAsync([FromRoute] long id, [FromRoute] long sourceId, [FromBody] PostAdjustWebsiteInputModel model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(model.PriceAdjustment))
                return UnprocessableEntity("PriceAdjustment is required.");

            var customPriceAdjustment = model.PriceAdjustment.Replace("+", "").Replace("-", "").Replace("%", "");
            if (!decimal.TryParse(customPriceAdjustment, out var tempCustomPriceAdjustment))
                return Json(UnprocessableEntity("PriceAdjustment is not well-formed."));

            if (string.IsNullOrEmpty(model.WholesalePriceAdjustment))
                return UnprocessableEntity("WholesalePriceAdjustment is required.");

            var customRetailPriceAdjustment = model.WholesalePriceAdjustment.Replace("+", "").Replace("-", "").Replace("%", "");
            if (!decimal.TryParse(customRetailPriceAdjustment, out var tempCustomRetailPriceAdjustment))
                return Json(UnprocessableEntity("WholesalePriceAdjustment is not well-formed."));

            var product = await _productService.GetByIdAsync(id, cancellationToken);

            var source = product.Sources.Single(x => x.Id == sourceId);
            if (source.SourceType != SourceType.Website)
                return BadRequest("Source type is not website");

            var newMetadata = JsonSerializer.Deserialize<WebsiteMetadataModel>(source.Metadata);

            var adjustedMetadata = _commonHelper.WebsitePriceAdjustment(newMetadata, model.PriceAdjustment, model.WholesalePriceAdjustment);

            var result = new PostAdjustWebsiteViewModel();
            result.Price = adjustedMetadata.Price;
            result.WholesalePrices = adjustedMetadata.WholesalePrices;

            return Ok(result);
        }

        [HttpPost("{id}/source/{sourceId}/adjustTelegramModel")]
        public async Task<ActionResult<PostAdjustTelegramViewModel>> PostAdjustTelegramAsync([FromRoute] long id, [FromRoute] long sourceId, [FromBody] PostAdjustTelegramInputModel model, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(model.PriceAdjustment))
                return UnprocessableEntity("PriceAdjustment is required.");

            var customPriceAdjustment = model.PriceAdjustment.Replace("+", "").Replace("-", "").Replace("%", "");
            if (!decimal.TryParse(customPriceAdjustment, out var tempCustomPriceAdjustment))
                return Json(UnprocessableEntity("PriceAdjustment is not well-formed."));

            if (model.Price <= 0)
                return Json(UnprocessableEntity("Price is invalid."));

            var product = await _productService.GetByIdAsync(id, cancellationToken);

            var source = product.Sources.Single(x => x.Id == sourceId);
            if (source.SourceType != SourceType.Telegram)
                return BadRequest("Source type is not telegram");

            var result = new PostAdjustTelegramViewModel();
            result.Price = _commonHelper.CustomPriceAdjustment(model.Price * _commonHelper.FixedAdjustmentRatio, model.PriceAdjustment);

            return Ok(result);
        }
    }
}
