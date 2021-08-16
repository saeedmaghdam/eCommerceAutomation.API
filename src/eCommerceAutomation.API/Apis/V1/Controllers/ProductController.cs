using System;
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
        private readonly ISourceService _sourceService;
        private readonly CommonHelper _commonHelper;

        public ProductController(IProductService productService, ISourceService sourceService, CommonHelper commonHelper)
        {
            _productService = productService;
            _sourceService = sourceService;
            _commonHelper = commonHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAsync(bool? isReviewNeeded, bool? isDisabled, bool? isInitialized, bool? isSourcesDisabled, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAsync(isReviewNeeded, isDisabled, isInitialized, isSourcesDisabled, cancellationToken);
            if (!products.Any())
                return NoContent();

            return Ok(ToViewModel(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(id, cancellationToken);
            
            return Ok(ToViewModel(new[] { product }).Single());
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

        [HttpPatch("{id}/disable")]
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
            await _productService.PatchProductAsync(id, model.Name, model.OriginalMinimumQuantity, model.OriginalPrice, model.OriginalWholesalePrices, model.MinimumQuantity, model.Price, model.WholesalePrices, model.IsReviewNeeded, model.IsInitialized, null, cancellationToken);

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

        [HttpPost("{id}/source/type/website/{sourceId}")]
        public async Task<ActionResult> UpdateWebsiteSourceAsync([FromRoute] long id, [FromRoute] long sourceId, [FromBody] PostUpdateWebsiteSource model, CancellationToken cancellationToken)
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

            if (product.IsDisabled && model.IsDisabled)
                return Json(UnprocessableEntity("Product already is disabled."));

            var newMetadata = JsonSerializer.Deserialize<WebsiteMetadataModel>(source.Metadata);

            var adjustedMetadata = _commonHelper.WebsitePriceAdjustment(newMetadata, source.PriceAdjustment, source.WholesalePriceAdjustment);

            if (!product.IsDisabled && source.IsDisabled)
            {
                var response = await _commonHelper.UnavailableProduct(product.ExternalId);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return Json(UnprocessableEntity($"API ERROR, HTTP Status Code: {response.StatusCode.ToString()}({(int)response.StatusCode})"));
            }
            else if (!newMetadata.IsInStock)
            {
                var response = await _commonHelper.UnavailableProduct(product.ExternalId);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    return Json(UnprocessableEntity($"API ERROR, HTTP Status Code: {response.StatusCode.ToString()}({(int)response.StatusCode})"));
            }
            else
            {
                if (!product.IsDisabled || product.IsDisabled && !source.IsDisabled)
                {
                    var websiteUpdateModel = new WebsiteMetadataModel();
                    websiteUpdateModel.Price = adjustedMetadata.Price;
                    websiteUpdateModel.MinimumQuantity = adjustedMetadata.MinimumQuantity;
                    websiteUpdateModel.WholesalePrices = adjustedMetadata.WholesalePrices;

                    var response = await _commonHelper.UpdateProductUsingWebsiteMetadataModel(product.ExternalId, websiteUpdateModel);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return Json(UnprocessableEntity($"API ERROR, HTTP Status Code: {response.StatusCode.ToString()}({(int)response.StatusCode})"));
                }
            }

            await _productService.PatchProductAsync(product.Id,
                null,
                adjustedMetadata.MinimumQuantity,
                newMetadata.Price,
                JsonSerializer.Serialize(newMetadata.WholesalePrices),
                adjustedMetadata.MinimumQuantity,
                adjustedMetadata.Price,
                JsonSerializer.Serialize(adjustedMetadata.WholesalePrices),
                false,
                true,
                model.IsDisabled,
                cancellationToken);

            await _sourceService.PatchAsync(source.Id,
                null,
                null,
                null,
                source.Metadata,
                null,
                model.PriceAdjustment,
                model.WholesalePriceAdjustment,
                cancellationToken);

            return Ok();
        }

        [HttpPost("{id}/source/type/telegram/{sourceId}")]
        public async Task<ActionResult> UpdateTelegram([FromRoute] long id, [FromRoute] long sourceId, [FromBody] PostUpdateTelegramSource model, CancellationToken cancellationToken)
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

            try
            {
                if (!product.IsDisabled && model.IsDisabled)
                {
                    var response = await _commonHelper.UnavailableProduct(product.ExternalId);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return Json(UnprocessableEntity($"API ERROR, HTTP Status Code: {response.StatusCode.ToString()}({(int)response.StatusCode})"));
                }
                else if (!product.IsDisabled || (product.IsDisabled && !model.IsDisabled))
                {
                    var websiteUpdateModel = new WebsiteMetadataModel();
                    websiteUpdateModel.Price = _commonHelper.CustomPriceAdjustment(model.Price * _commonHelper.FixedAdjustmentRatio, model.PriceAdjustment);
                    websiteUpdateModel.MinimumQuantity = model.MinimumQuantity;

                    var response = await _commonHelper.UpdateProductUsingWebsiteMetadataModel(product.ExternalId, websiteUpdateModel);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return Json(UnprocessableEntity($"API ERROR, HTTP Status Code: {response.StatusCode.ToString()}({(int)response.StatusCode})"));
                }

                await _productService.PatchProductAsync(product.Id,
                null,
                model.MinimumQuantity,
                model.Price,
                null,
                model.MinimumQuantity,
                _commonHelper.CustomPriceAdjustment(model.Price * _commonHelper.FixedAdjustmentRatio, model.PriceAdjustment),
                null,
                false,
                true,
                model.IsDisabled,
                cancellationToken);

                await _sourceService.PatchAsync(source.Id,
                    null,
                    null,
                    null,
                    source.Metadata,
                    null,
                    model.PriceAdjustment,
                    null,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity($"API ERROR: {ex.ToString()}.");
            }

            return Ok();
        }

        private IEnumerable<ProductViewModel> ToViewModel(IEnumerable<IProduct> products)
        {
            return products.Select(product => new ProductViewModel()
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
            });
        }
    }
}
