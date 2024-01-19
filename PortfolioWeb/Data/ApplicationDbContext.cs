using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Models;

namespace PortfolioWeb.Data
{
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<PortfolioWeb.Models.BlogPost> BlogPost { get; set; } = default!;
        public DbSet<PortfolioWeb.Models.Project> Project { get; set; } = default!;
        public DbSet<PortfolioWeb.Models.Tag> Tag { get; set; } = default!;
    }
}
