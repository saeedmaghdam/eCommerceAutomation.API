using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Domain;
using eCommerceAutomation.API.Framework.Services.Product;
using eCommerceAutomation.API.Framework.Services.Product.ServiceInputModel;

namespace eCommerceAutomation.API.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db) => _db = db;

        public async Task<IProduct> CreateAsync(string externalId, string name, string url, IEnumerable<SourceServiceInputModel> sources, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(externalId))
                throw new System.Exception("External Id is required.");

            if (string.IsNullOrEmpty(name))
                throw new System.Exception("Name is required.");

            if (string.IsNullOrEmpty(url))
                throw new System.Exception("Url is required.");

            if (!sources.Any())
                throw new System.Exception("At least one source is required.");

            foreach (var source in sources)
            {
                if (string.IsNullOrEmpty(source.Address))
                    throw new System.Exception("Source address is required.");
            }

            var newProduct = Domain.Product.Create();
            newProduct.ExternalId = externalId;
            newProduct.Url = url;
            newProduct.Name = name;
            newProduct.Sources = sources.Select(source => new Domain.Source()
            {
                Address = source.Address,
                PriceAdjustment = source.PriceAdjustment,
                Priority = source.Priority,
                RecordInsertDateTime = DateTime.Now,
                RecordStatus = Framework.Constants.RecordStatus.Inserted,
                Product = newProduct,
                RecordUpdateDateTime = DateTime.Now,
                SourceType = source.SourceType,
                ViewId = Guid.NewGuid(),
                WholesalePriceAdjustment = source.WholesalePriceAdjustment
            }).ToList();
            _db.Products.Add(newProduct);

            await _db.SaveChangesAsync(cancellationToken);

            return ToModel(new[] { newProduct }).Single();
        }

        public static IEnumerable<IProduct> ToModel(IEnumerable<Domain.Product> products)
        {
            return products.Select(product => new ProductModel()
            {
                RecordStatus = product.RecordStatus,
                ExternalId = product.ExternalId,
                Id = product.Id,
                IsInitialized = product.IsInitialized,
                IsDisabled = product.IsDisabled,
                IsReviewNeeded = product.IsReviewNeeded,
                MinimumQuantity = product.MinimumQuantity,
                Name = product.Name,
                OriginalMinimumQuantity = product.OriginalMinimumQuantity,
                OriginalPrice = product.OriginalPrice,
                OriginalWholesalePrices = product.OriginalWholesalePrices,
                Price = product.Price,
                RecordInsertDateTime = product.RecordInsertDateTime,
                RecordUpdateDateTime = product.RecordUpdateDateTime,
                Url = product.Url,
                ViewId = product.ViewId,
                WholesalePrices = product.WholesalePrices,
                Sources = product.Sources == null ? null : product.Sources.Select(source => new SourceModel()
                {
                    SourceType = source.SourceType,
                    Address = source.Address,
                    RecordStatus = source.RecordStatus,
                    Id = source.Id,
                    Metadata = source.Metadata,
                    OldMetadata = source.OldMetadata,
                    PriceAdjustment = source.PriceAdjustment,
                    Priority = source.Priority,
                    RecordInsertDateTime = source.RecordInsertDateTime,
                    RecordUpdateDateTime = source.RecordUpdateDateTime,
                    ViewId = source.ViewId,
                    WholesalePriceAdjustment = source.WholesalePriceAdjustment
                })
            });
        }
    }
}
