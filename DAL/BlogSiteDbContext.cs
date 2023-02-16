using Microsoft.EntityFrameworkCore;

namespace DAL;

public class BlogSiteDbContext : DbContext
{
    public BlogSiteDbContext(DbContextOptions<BlogSiteDbContext> options) : base(options)
    {
        
    }

    public BlogSiteDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=localhost,1433;database=BlogSiteDb;uid=sa;pwd=yourStrongPassword123;TrustServerCertificate=True");
    }
    public DbSet<Sex> Sexes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
}