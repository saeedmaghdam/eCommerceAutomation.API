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

        public DbSet<Product> Products
        {
            get;
            set;
        }

        public DbSet<Source> Sources
        {
            get;
            set;
        }

        public DbSet<SourceName> SourceNames
        {
            get;
            set;
        }
    }
}
