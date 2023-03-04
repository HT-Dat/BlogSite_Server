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
    public DbSet<Sex> Sexes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<PostStatus> PostStatusEnumerable { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    
}