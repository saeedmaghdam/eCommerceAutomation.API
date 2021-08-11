using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using eCommerceAutomation.API.Domain;
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
            var source = await _db.Sources.Where(source => source.Id == id && source.RecordStatus != Framework.Constants.RecordStatus.Deleted).SingleOrDefaultAsync();
            if (source == null)
                throw new Exception("Not found.");

            source.IsDisabled = isDisabled;

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
