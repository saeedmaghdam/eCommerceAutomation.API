using Microsoft.EntityFrameworkCore;

namespace eCommerceAutomation.API.Domain
{
    public class AppDbContext : DbContext
    {
        private readonly DbContextOptions _options;

        public AppDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        public AppDbContext Create()
        {
            return new AppDbContext(_options);
        }
    }
}
