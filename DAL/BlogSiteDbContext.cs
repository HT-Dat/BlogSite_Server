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
        optionsBuilder.UseSqlServer("server=db,1433;database=BlogSiteDb;uid=sa;pwd=myStrongPassword123;");
    }
    public DbSet<Sex> Sexes { get; set; }
    public DbSet<User> Users { get; set; }
}