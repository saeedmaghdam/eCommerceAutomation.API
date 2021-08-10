using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Domain;
using eCommerceAutomation.API.Framework.Constants;
using eCommerceAutomation.API.Framework.Services.Product;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAutomation.API.Service.Product
{
    public class SourceNameService : ISourceNameService
    {
        private readonly AppDbContext _db;

        public SourceNameService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<ISourceName>> GetAsync(SourceType? sourceType, CancellationToken cancellationToken)
        {
            var query = _db.SourceNames.Where(x => x.RecordStatus != Framework.Constants.RecordStatus.Deleted).AsQueryable();
            if (sourceType.HasValue)
                query = query.Where(x => x.SourceType == sourceType);

            var sourceNames = await query.ToListAsync();

            return sourceNames.Select(sourceName => new SourceNameModel()
            {
                SourceType = sourceName.SourceType,
                Key = sourceName.Key
            });
        }
    }
}
