using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence;

public interface IBlogSiteDbContext
{
    DbSet<Sex> Sexes { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Tag> Tags { get; set; }
    DbSet<Comment> Comments { get; set; }
    DbSet<Post> Posts { get; set; }
    DbSet<PostCategory> PostCategories { get; set; }
    DbSet<PostStatus> PostStatusEnumerable { get; set; }
    DbSet<PostTag> PostTags { get; set; }

    void SetModified(object entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}