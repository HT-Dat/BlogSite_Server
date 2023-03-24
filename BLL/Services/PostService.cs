using AutoMapper;
using BLL.Services.IServices;
using DAL;
using DAL.Entities;
using DTO.DTOs;

namespace BLL.Services;

public class PostService : IPostService
{
    private readonly BlogSiteDbContext _blogSiteDbContext;
    private readonly IMapper _mapper;

    public PostService(BlogSiteDbContext blogSiteDbContext, IMapper mapper)
    {
        _blogSiteDbContext = blogSiteDbContext;
        _mapper = mapper;
    }

    public async Task<PostToReturnDto> Get(int id)
    {
        var post = await _blogSiteDbContext.Posts.FindAsync(id);
        return _mapper.Map<PostToReturnDto>(post);
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

    public async Task<PostToReturnDto> Update(PostToUpdate postToUpdate)
    {
        var updatingPost = _mapper.Map<Post>(postToUpdate);
        updatingPost.UpdatedDate = DateTime.UtcNow;
        _blogSiteDbContext.Entry<Post>(updatingPost).Property(o => o.Content).IsModified = true;
        _blogSiteDbContext.Entry<Post>(updatingPost).Property(o => o.Title).IsModified = true;
        _blogSiteDbContext.Entry<Post>(updatingPost).Property(o => o.StatusId).IsModified = true;
        _blogSiteDbContext.Entry<Post>(updatingPost).Property(o => o.UpdatedDate).IsModified = true;
        
        await _blogSiteDbContext.SaveChangesAsync();
        return _mapper.Map<PostToReturnDto>(updatingPost);
    }
}