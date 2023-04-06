using System.Net;
using System.Text.RegularExpressions;
using AutoMapper;
using BLL.Services.IServices;
using DAL.Persistence;
using DAL.Entities;
using DTO.DTOs;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class PostService : IPostService
{
    private readonly BlogSiteDbContext _blogSiteDbContext;
    private readonly IMapper _mapper;
    private readonly byte DRAFT = 0;
    private readonly byte PENDING = 1;
    private readonly byte PUBLISHED = 2;

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

    public async Task<PostToReturnPublicDto> GetPublic(string permalink)
    {
        var post = await _blogSiteDbContext.Posts.Where(x => ((x.Permalink == permalink) && (x.StatusId == PUBLISHED)))
            .Include(o => o.Author).FirstOrDefaultAsync();
        return _mapper.Map<PostToReturnPublicDto>(post);
    }

    public async Task<List<PostToReturnForListDto>> GetList(string authorId)
    {
        var listPostFromDb = await _blogSiteDbContext.Posts.Where(o => o.Author.Id == authorId).ToListAsync();
        var mappedList = _mapper.Map<List<Post>, List<PostToReturnForListDto>>(listPostFromDb);
        return mappedList;
    }

    public async Task<List<PostToReturnForListPublicDto>> GetListPublic()
    {
        var listPostFromDb = await _blogSiteDbContext.Posts.Where(x => x.StatusId == PUBLISHED).Include(o => o.Author)
            .ToListAsync();
        var mappedList = _mapper.Map<List<Post>, List<PostToReturnForListPublicDto>>(listPostFromDb);
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
            Permalink = string.Empty,
            Preview = string.Empty,
            ThumbnailUrl = String.Empty,
        };
        await _blogSiteDbContext.Posts.AddAsync(addedPost);
        await _blogSiteDbContext.SaveChangesAsync();
        return addedPost;
    }

    public async Task Delete(int id, string authorId)
    {
        if (_blogSiteDbContext.Posts.Any(x => (x.Id == id) && (x.AuthorId == authorId)))
        {
            var deletingPost = new Post
            {
                Id = id
            };
            _blogSiteDbContext.Posts.Attach(deletingPost);
            _blogSiteDbContext.Posts.Remove(deletingPost);
        }

        await _blogSiteDbContext.SaveChangesAsync();
    }

    public async Task<PostToReturnDto> Update(PostToUpdateDto postToUpdate, string authorId)
    {
        if (_blogSiteDbContext.Posts.Any(x => (x.Id == postToUpdate.Id) && (x.AuthorId == authorId)) == false)
        {
            return new PostToReturnDto();
        }

        var updatingPost = _mapper.Map<Post>(postToUpdate);
        updatingPost.UpdatedDate = DateTime.UtcNow;
        updatingPost.Preview = GetPreview(updatingPost.Content);
        updatingPost.ThumbnailUrl = GetThumbnailUrl(updatingPost.Content);

        var isAuthorAdmin = await _blogSiteDbContext.Users.Where(o => o.Id == authorId).Select(o => o.IsAdmin)
            .FirstOrDefaultAsync();
        if (isAuthorAdmin == false && updatingPost.StatusId != null)
        {
            if (updatingPost.StatusId > 0)
            {
                updatingPost.StatusId = PENDING;
            }
        }

        if (updatingPost.StatusId != 0)
        {
            updatingPost.PublishedDate = DateTime.UtcNow;
        }

        if ((string.IsNullOrWhiteSpace(updatingPost.Title) == false) &&
            (string.IsNullOrWhiteSpace(updatingPost.Permalink) == false))
        {
            updatingPost.Permalink = await EncodeAndValidatePermalink(updatingPost.Permalink, updatingPost.Id);
        }
        else if (string.IsNullOrWhiteSpace(updatingPost.Title))
        {
            updatingPost.Permalink = await EncodeAndValidatePermalink(updatingPost.Permalink, updatingPost.Id);
        }
        else if (string.IsNullOrWhiteSpace(updatingPost.Permalink))
        {
            updatingPost.Permalink = await EncodeAndValidatePermalink(updatingPost.Title, updatingPost.Id);
        }

        foreach (var propInfo in typeof(Post).GetProperties())
        {
            var postPropValue = propInfo.GetValue(updatingPost);
            if (postPropValue != null && propInfo.Name != "Id")
            {
                if ((postPropValue is DateTime time && time == default(DateTime)) == false)
                {
                    _blogSiteDbContext.Entry<Post>(updatingPost).Property(propInfo.Name).IsModified = true;
                }
            }
        }

        await _blogSiteDbContext.SaveChangesAsync();
        return _mapper.Map<PostToReturnDto>(updatingPost);
    }

    private string GetPreview(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(input);

        var fullPost = htmlDoc.DocumentNode.InnerText;
        string[] words = fullPost.Split(' ');
        return string.Join(" ", words.Take(50).ToArray());
    }

    private string GetThumbnailUrl(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(input);
        var firstImageNode = htmlDoc.DocumentNode.SelectSingleNode("//img");
        if (firstImageNode == null)
        {
            return string.Empty;
        }

        return firstImageNode.GetAttributeValue("src", string.Empty);
    }

    private async Task<string> EncodeAndValidatePermalink(string input, int idPostReceivingFromFe)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }

        input = input.Trim();
        input = input.ToLower();
        //remove special character
        var encodedUrl = Regex.Replace(input, @"[^\w\s-]", "");
        //remove white space
        encodedUrl = encodedUrl.Replace(" ", "-");
        //remove --
        while (Regex.IsMatch(encodedUrl, @"--"))
        {
            encodedUrl = Regex.Replace(encodedUrl, @"--", "-");
        }

        //check if exist post that have the same permalink
        var postGetFromDb = await _blogSiteDbContext.Posts.Where(o => o.Permalink == encodedUrl).AsNoTracking()
            .FirstOrDefaultAsync();
        if (postGetFromDb != null)
        {
            if (postGetFromDb.Id != idPostReceivingFromFe)
            {
                //Make numericDate and append to permalink
                encodedUrl += $"-{GetCurrentTimeAdNumericDate()}";
            }
        }


        return encodedUrl;
    }

    private string GetCurrentTimeAdNumericDate()
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSinceEpoch = DateTime.UtcNow - unixEpoch;
        return ((long)timeSinceEpoch.TotalSeconds).ToString();
    }
}