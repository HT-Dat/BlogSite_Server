using AutoMapper;
using BLL.Services.IServices;
using DAL;
using DAL.Entities;
using DTO.DTOs;

namespace BLL.Services;

public class PostService :IPostService
{
    private readonly BlogSiteDbContext _blogSiteDbContext;
    private readonly IMapper _mapper;

    public PostService(BlogSiteDbContext blogSiteDbContext, IMapper mapper)
    {
        _blogSiteDbContext = blogSiteDbContext;
        _mapper = mapper;
    }

    public Task<Post> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Post> Add(string authorId)
    {
        var addedPost = new Post
        {
            Content = String.Empty,
            Title = string.Empty,
            AuthorId = authorId,
            CreatedDate = DateTime.UtcNow
        };
        await _blogSiteDbContext.Posts.AddAsync(addedPost);
        await _blogSiteDbContext.SaveChangesAsync();
        return addedPost;
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(Post post)
    {
        throw new NotImplementedException();
    }
}