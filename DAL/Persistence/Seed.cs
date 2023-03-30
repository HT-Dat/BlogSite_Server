using DAL.Entities;

namespace DAL.Persistence;

public class Seed
{
    public static async Task SeedData(BlogSiteDbContext blogSiteDbContext)
    {
        if ((blogSiteDbContext.Sexes.Any() || blogSiteDbContext.PostStatusEnumerable.Any()) == false)
        {
            var sexes = new List<Sex>
            {
                new Sex
                {
                    Id = 0,
                    Name = "Not known"
                },
                new Sex
                {
                    Id = 1,
                    Name = "Male"
                },
                new Sex
                {
                    Id = 2,
                    Name = "Female"
                }
            };
            var postStatuses = new List<PostStatus>
            {
                new PostStatus()
                {
                    Id = 0,
                    Name = "Draft"
                },
                new PostStatus()
                {
                    Id = 1,
                    Name = "Pending"
                },
                new PostStatus()
                {
                    Id = 2,
                    Name = "Published"
                },
                new PostStatus()
                {
                    Id = 3,
                    Name = "Deleted"
                }
            };
            await blogSiteDbContext.Sexes.AddRangeAsync(sexes);
            await blogSiteDbContext.PostStatusEnumerable.AddRangeAsync(postStatuses);
            await blogSiteDbContext.SaveChangesAsync();
        }
    }
}