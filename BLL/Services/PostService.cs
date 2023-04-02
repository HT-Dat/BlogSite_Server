using AutoMapper;
using BLL.Services.IServices;
using DAL.Persistence;
using DAL.Entities;
using DTO.DTOs;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<PostToReturnForListDto>> GetList()
    {
        var listPostFromDb = await _blogSiteDbContext.Posts.ToListAsync();
        var mappedList = _mapper.Map<List<Post>, List<PostToReturnForListDto>>(listPostFromDb);
        return mappedList;
    }

    public async Task<Post> Add(string authorId)
    {
        var addedPost = new Post
        {
            Content = string.Empty,
            Title = string.Empty,
            AuthorId = authorId,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            StatusId = 0,
            Permalink = string.Empty
        };
        await _blogSiteDbContext.Posts.AddAsync(addedPost);
        await _blogSiteDbContext.SaveChangesAsync();
        return addedPost;
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<PostToReturnDto> Update(PostToUpdateDto postToUpdate)
    {
        var updatingPost = _mapper.Map<Post>(postToUpdate);
        updatingPost.UpdatedDate = DateTime.UtcNow;

        foreach (var propInfo in typeof(Post).GetProperties())
        {
            var postPropValue = propInfo.GetValue(updatingPost);
            if (postPropValue != null && propInfo.Name != "Id")
            {
                if (postPropValue is string)
                {
                    _blogSiteDbContext.Entry<Post>(updatingPost).Property(propInfo.Name).IsModified = true;
                }
                else if (postPropValue is DateTime time && time != default(DateTime))
                {
                    _blogSiteDbContext.Entry<Post>(updatingPost).Property(propInfo.Name).IsModified = true;
                }
            }
        }
        await _blogSiteDbContext.SaveChangesAsync();
        return _mapper.Map<PostToReturnDto>(updatingPost);
    }
}