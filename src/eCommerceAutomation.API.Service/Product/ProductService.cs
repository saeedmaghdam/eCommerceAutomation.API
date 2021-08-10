﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Domain;
using eCommerceAutomation.API.Framework.Services.Product;
using eCommerceAutomation.API.Framework.Services.Product.ServiceInputModel;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAutomation.API.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<IProduct>> GetAsync(bool? isReviewNeeded, bool? isDisabled, bool? isInitialized, bool? isSourcesDisabled, CancellationToken cancellationToken)
        {
            var query = _db.Products.Where(x => x.RecordStatus != Framework.Constants.RecordStatus.Deleted).AsQueryable();

            if (isSourcesDisabled.HasValue)
                query = query.Include(x => x.Sources.Where(z => z.IsDisabled == isSourcesDisabled.Value));
            else
                query = query.Include(x => x.Sources);

            if (isReviewNeeded.HasValue)
                query = query.Where(x => x.IsReviewNeeded == isReviewNeeded.Value);

            if (isDisabled.HasValue)
                query = query.Where(x => x.IsDisabled == isDisabled.Value);

            if (isInitialized.HasValue)
                query = query.Where(x => x.IsInitialized == isInitialized.Value);

            var items = await query.ToListAsync(cancellationToken);

            return ToModel(items);
        }

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

                try
                {
                    JsonSerializer.Deserialize<string[]>(source.Address);
                }
                catch
                {
                    throw new Exception("Source address is not presented as a valid json.");
                }
            }

            var newProduct = Domain.Product.Create();
            newProduct.ExternalId = externalId;
            newProduct.Url = url;
            newProduct.Name = name;
            newProduct.Sources = sources.Select(source => new Domain.Source()
            {
                Address = JsonSerializer.Serialize(JsonSerializer.Deserialize<string[]>(source.Address).Select(x => x.Trim())),
                PriceAdjustment = source.PriceAdjustment,
                Priority = source.Priority,
                RecordInsertDateTime = DateTime.Now,
                RecordStatus = Framework.Constants.RecordStatus.Inserted,
                Product = newProduct,
                RecordUpdateDateTime = DateTime.Now,
                SourceType = source.SourceType,
                ViewId = Guid.NewGuid(),
                WholesalePriceAdjustment = source.WholesalePriceAdjustment,
                IsDisabled = false,
                Key = source.Key
            }).ToList();
            _db.Products.Add(newProduct);

            await _db.SaveChangesAsync(cancellationToken);

            return ToModel(new[] { newProduct }).Single();
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var product = await _db.Products.Where(x => x.Id == id && x.RecordStatus != Framework.Constants.RecordStatus.Deleted).SingleOrDefaultAsync();
            if (product == null)
                throw new Exception("Not found.");

            product.RecordStatus = Framework.Constants.RecordStatus.Deleted;
            product.RecordUpdateDateTime = DateTime.Now;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task PutSourcesAsync(long id, IEnumerable<SourceServiceInputModel> sources, CancellationToken cancellationToken)
        {
            var product = await _db.Products.Include(x => x.Sources).Where(x => x.Id == id && x.RecordStatus != Framework.Constants.RecordStatus.Deleted).SingleOrDefaultAsync();
            if (product == null)
                throw new Exception("Not found.");

            foreach (var source in sources)
            {
                if (string.IsNullOrEmpty(source.Address))
                    throw new System.Exception("Source address is required.");

                try
                {
                    JsonSerializer.Deserialize<string[]>(source.Address);
                }
                catch
                {
                    throw new Exception("Source address is not presented as a valid json.");
                }
            }

            var now = DateTime.Now;
            foreach (var source in product.Sources)
            {
                source.RecordStatus = Framework.Constants.RecordStatus.Deleted;
                source.RecordUpdateDateTime = now;
            }

            foreach (var source in sources)
            {
                var address = JsonSerializer.Deserialize<string[]>(source.Address).Select(x => x.Trim());
                var newAddress = JsonSerializer.Serialize(address);

                if (source.Id.HasValue)
                {
                    var currentSource = product.Sources.Where(x => x.Id == source.Id.Value).SingleOrDefault();

                    if (currentSource == null)
                        throw new Exception("Source not found.");

                    currentSource.Address = newAddress;
                    currentSource.PriceAdjustment = source.PriceAdjustment;
                    currentSource.Priority = source.Priority;
                    currentSource.WholesalePriceAdjustment = source.WholesalePriceAdjustment;
                    currentSource.RecordStatus = Framework.Constants.RecordStatus.Updated;
                    currentSource.RecordUpdateDateTime = now;
                    currentSource.Key = source.Key;
                }
                else
                {
                    var newSource = Domain.Source.Create();
                    newSource.Address = newAddress;
                    newSource.PriceAdjustment = source.PriceAdjustment;
                    newSource.Priority = source.Priority;
                    newSource.WholesalePriceAdjustment = source.WholesalePriceAdjustment;
                    newSource.SourceType = source.SourceType;
                    newSource.Key = source.Key;

                    product.Sources.Add(newSource);
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task PatchStatusAsync(long id, bool isDisabled, CancellationToken cancellationToken)
        {
            var product = await _db.Products.Where(x => x.Id == id && x.RecordStatus != Framework.Constants.RecordStatus.Deleted).SingleOrDefaultAsync();
            if (product == null)
                throw new Exception("Not found.");

            product.IsDisabled = isDisabled;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task PatchReviewNeededStatusAsync(long id, bool isReviewNeeded, CancellationToken cancellationToken)
        {
            var product = await _db.Products.Where(x => x.Id == id && x.RecordStatus != Framework.Constants.RecordStatus.Deleted).SingleOrDefaultAsync();
            if (product == null)
                throw new Exception("Not found.");

            product.IsReviewNeeded = isReviewNeeded;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task PatchSourceStatusAsync(long sourceId, bool isDisabled, CancellationToken cancellationToken)
        {
            var source = await _db.Sources.Where(x => x.Id == sourceId && x.RecordStatus != Framework.Constants.RecordStatus.Deleted).SingleOrDefaultAsync();
            if (source == null)
                throw new Exception("Not found.");

            source.IsDisabled = isDisabled;

            await _db.SaveChangesAsync(cancellationToken);
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
                    WholesalePriceAdjustment = source.WholesalePriceAdjustment,
                    IsDisabled = source.IsDisabled,
                    Key = source.Key
                })
            });
        }
    }
}
