using Microsoft.EntityFrameworkCore;
using BackEndAPI.Models;

namespace BackEndAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BlogPost> Blogs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>()
                .HasMany(b => b.Tags)
                .WithMany(t => t.Blogs);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Projects);
        }
    }    
}
