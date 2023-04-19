using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence;

public class BlogSiteDbContext : DbContext, IBlogSiteDbContext
{
    //dotnet ef migrations add ChangePublishedDateToNullable --project ../  --startup-project ../../WebAPI/
    public BlogSiteDbContext(DbContextOptions<BlogSiteDbContext> options) : base(options)
    {
    }

    public BlogSiteDbContext()
    {
    }

    public virtual DbSet<Sex> Sexes { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<PostCategory> PostCategories { get; set; }
    public virtual DbSet<PostStatus> PostStatusEnumerable { get; set; }
    public virtual DbSet<PostTag> PostTags { get; set; }
    public void SetModified(object entity)
    {
        Entry(entity).State = EntityState.Modified;
    }
}