using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Domain;
using eCommerceAutomation.Framework.Constants;
using eCommerceAutomation.API.Framework.Services.Product;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAutomation.API.Service.Product
{
    public class SourceService : ISourceService
    {
        private readonly AppDbContext _db;

        public SourceService(AppDbContext db) => _db = db;

        public async Task PatchStatusAsync(long id, bool isDisabled, CancellationToken cancellationToken)
        {
            var source = await _db.Sources.Where(source => source.Id == id && source.RecordStatus != RecordStatus.Deleted).SingleOrDefaultAsync();
            if (source == null)
                throw new Exception("Not found.");

            source.IsDisabled = isDisabled;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task PatchAsync(long id, int? priority, SourceType? sourceType, string address, string oldMetadata, string metadata, string priceAdjustment, string wholesalePriceAdjustment, CancellationToken cancellationToken)
        {
            var source = await _db.Sources.Where(source => source.Id == id && source.RecordStatus != RecordStatus.Deleted).SingleOrDefaultAsync();
            if (source == null)
                throw new Exception("Not found.");

            if (priority.HasValue)
                source.Priority = priority.Value;

            if (sourceType.HasValue)
                source.SourceType = sourceType.Value;

            if (!string.IsNullOrEmpty(address))
                source.Address = address;

            if (!string.IsNullOrEmpty(oldMetadata))
                source.OldMetadata = oldMetadata;

            if (!string.IsNullOrEmpty(metadata))
                source.Metadata = metadata;

            if (!string.IsNullOrEmpty(priceAdjustment))
                source.PriceAdjustment = priceAdjustment;

            if (!string.IsNullOrEmpty(wholesalePriceAdjustment))
                source.WholesalePriceAdjustment = wholesalePriceAdjustment;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task ActiveSourceAsync(long id, CancellationToken cancellationToken)
        {
            var source = await _db.Sources.Where(source => source.Id == id && source.RecordStatus != RecordStatus.Deleted).SingleOrDefaultAsync();
            if (source == null)
                throw new Exception("Not found.");

            foreach (var item in await _db.Sources.Where(x => x.RecordStatus != RecordStatus.Deleted).ToListAsync(cancellationToken))
                item.IsActive = false;

            source.IsActive = true;

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
