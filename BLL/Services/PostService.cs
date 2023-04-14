using System.Net;
using System.Text.RegularExpressions;
using AutoMapper;
using BLL.Services.IServices;
using BLL.Utilities;
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
    private readonly ITimeHelper _timeHelper;

    private readonly byte DRAFT = 0;
    private readonly byte PENDING = 1;
    private readonly byte PUBLISHED = 2;

    public PostService(BlogSiteDbContext blogSiteDbContext, IMapper mapper, ITimeHelper timeHelper)
    {
        _blogSiteDbContext = blogSiteDbContext;
        _mapper = mapper;
        _timeHelper = timeHelper;
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

    private async Task<Post> GenerateUpdatingPostWithFullData(PostToUpdateDto postToUpdateDto, string authorId)
    {
        var updatingPostFullData = _mapper.Map<Post>(postToUpdateDto);
        updatingPostFullData.UpdatedDate = DateTime.UtcNow;
        updatingPostFullData.Preview = GetPreviewContent(updatingPostFullData.Content);
        updatingPostFullData.ThumbnailUrl = GetThumbnailUrl(updatingPostFullData.Content);
        var isAuthorAdmin = await _blogSiteDbContext.Users.Where(o => o.Id == authorId).Select(o => o.IsAdmin)
            .FirstOrDefaultAsync();
        if (isAuthorAdmin == false && updatingPostFullData.StatusId is > 0)
        {
            updatingPostFullData.StatusId = PENDING;
        }

        if (updatingPostFullData.StatusId != 0)
        {
            updatingPostFullData.PublishedDate = DateTime.UtcNow;
        }
        
        updatingPostFullData.Permalink =
            await EncodeAndValidatePermalink(
                GetUnencodedPermalink(updatingPostFullData.Title, updatingPostFullData.Permalink),
                updatingPostFullData.Id);
        return updatingPostFullData;
    }

    public async Task<PostToReturnDto> Update(PostToUpdateDto postToUpdate, string authorId)
    {
        if (_blogSiteDbContext.Posts.Any(x => (x.Id == postToUpdate.Id) && (x.AuthorId == authorId)) == false)
        {
            return new PostToReturnDto();
        }

        var updatingPostFullData = await GenerateUpdatingPostWithFullData(postToUpdate, authorId);

        foreach (var propInfo in typeof(Post).GetProperties())
        {
            var postPropValue = propInfo.GetValue(updatingPostFullData);
            if (postPropValue != null && propInfo.Name != "Id")
            {
                if ((postPropValue is DateTime time && time == default(DateTime)) == false)
                {
                    _blogSiteDbContext.Entry(updatingPostFullData).Property(propInfo.Name).IsModified = true;
                }
            }
        }

        await _blogSiteDbContext.SaveChangesAsync();
        return _mapper.Map<PostToReturnDto>(updatingPostFullData);
    }

    private string GetPreviewContent(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return string.Empty;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(content);

        var fullPost = htmlDoc.DocumentNode.InnerText;
        string[] words = fullPost.Split(' ');
        return string.Join(" ", words.Take(50).ToArray());
    }

    private string GetThumbnailUrl(string postContent)
    {
        if (string.IsNullOrEmpty(postContent))
        {
            return string.Empty;
        }

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(postContent);
        var firstImageNode = htmlDoc.DocumentNode.SelectSingleNode("//img");
        if (firstImageNode == null)
        {
            return string.Empty;
        }

        return firstImageNode.GetAttributeValue("src", string.Empty);
    }

    private string GetUnencodedPermalink(string title, string permalinkField)
    {
        var isTitleHaveData = !string.IsNullOrWhiteSpace(title);
        var isPermalinkFieldHaveData = !string.IsNullOrWhiteSpace(permalinkField);
        if (!isTitleHaveData && !isPermalinkFieldHaveData)
        {
            return string.Empty;
        }

        if (isPermalinkFieldHaveData)
        {
            return permalinkField;
        }

        return title;
    }

    private async Task<string> EncodeAndValidatePermalink(string unencodedPermalink, int postId)
    {
        if (string.IsNullOrEmpty(unencodedPermalink))
        {
            return string.Empty;
        }

        unencodedPermalink = unencodedPermalink.Trim().ToLower();
        //remove special character
        var encodedUrl = Regex.Replace(unencodedPermalink, @"[^\w\s-]", "");
        //remove white space
        encodedUrl = encodedUrl.Replace(" ", "-");
        //remove --
        while (Regex.IsMatch(encodedUrl, @"--"))
        {
            encodedUrl = Regex.Replace(encodedUrl, @"--", "-");
        }

        //check if exist post that have the same permalink
        var idPostHaveEncodedUrl = await _blogSiteDbContext.Posts.Where(o => o.Permalink == encodedUrl)
            .Select(o => o.Id)
            .FirstOrDefaultAsync();
        if (idPostHaveEncodedUrl != 0 && idPostHaveEncodedUrl != postId)
        {
            //Make numericDate and append to permalink
            var numericDate = _timeHelper.CurrentNumericDate();
            encodedUrl += $"-{numericDate}";
        }


        return encodedUrl;
    }
}