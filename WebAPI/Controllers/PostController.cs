using System.Security.Claims;
using BLL.Services.IServices;
using DAL.Entities;
using DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Storage.V1;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly StorageClient _storageClient;

    public PostController(IPostService postService, StorageClient storageClient)
    {
        _postService = postService;
        _storageClient = storageClient;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<ActionResult<Post>> PostPost()
    {
        var authorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var a = await _postService.Add(authorId);

        return Ok(a);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var postToReturnDto = await _postService.Get(id);

        return Ok(postToReturnDto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<ActionResult<List<PostToReturnForListDto>>> GetPost()
    {
        var list = await _postService.GetList();

        return Ok(list);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<ActionResult<Post>> PutPost(PostToUpdate postToUpdate)
    {
        var postToReturnDto = await _postService.Update(postToUpdate);
        return Ok(postToReturnDto);
    }

    [Authorize]
    [HttpPost("upload-editor-image")]
    public async Task<JsonResult> UploadCKEditorImage()
    {
        var files = Request.Form.Files;
        if (files.Count == 0)
        {
            var rError = new
            {
                uploaded = false,
                url = string.Empty
            };
            return Json(rError);
        }

        var formFile = files[0];
        var upFileName = formFile.FileName;

        var fileName = Path.GetFileNameWithoutExtension(upFileName) +
                       DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(upFileName);
        string bucketName = "hotiendat-blog.appspot.com";
        var bucket = _storageClient.GetBucket(bucketName);
        var obj = new Google.Apis.Storage.v1.Data.Object
        {
            Bucket = bucketName,
            Name = $"post_img/{fileName}",
            ContentType = formFile.ContentType
        };
        
        using (var memoryStream = new MemoryStream())
        {
            formFile.CopyTo(memoryStream);
            memoryStream.Position = 0;
            
            await _storageClient.UploadObjectAsync(obj, await Crop(memoryStream, 400, 300));
        }

        var previewPath =
            $"https://firebasestorage.googleapis.com/v0/b/{bucketName}/o/{Uri.EscapeDataString(obj.Name)}?alt=media";
        bool result = true;
        var rUpload = new
        {
            uploaded = result,
            url = result ? previewPath : string.Empty
        };
        return Json(rUpload);
    }

    private async Task<MemoryStream> Crop(MemoryStream stream, int maxWidth, int maxHeight)
    {
        Image image = await Image.LoadAsync(stream);
        var resizeOptions = new ResizeOptions()
        {
            Size = new Size(maxWidth, maxHeight),
            Mode = ResizeMode.Max
        };
        image.Mutate(x => x.Resize(resizeOptions));
        MemoryStream outputStream = new MemoryStream();
        await image.SaveAsync(outputStream, image.Metadata.DecodedImageFormat);
        return outputStream;
    }
}