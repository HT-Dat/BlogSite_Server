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
    private readonly IImageService _imageService;
    public PostController(IPostService postService, IImageService imageService)
    {
        _postService = postService;
        _imageService = imageService;
    }

    [HttpPost("my-content")]
    [Authorize]
    public async Task<ActionResult<Post>> PostPost()
    {
        var authorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var a = await _postService.Add(authorId);

        return Ok(a);
    }

    [HttpGet("my-content/{id}")]
    [Authorize]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var postToReturnDto = await _postService.Get(id);

        return Ok(postToReturnDto);
    }
    [HttpGet("public/{permalink}")]
    public async Task<ActionResult<PostToReturnPublicDto>> GetPostPublic(string permalink)
    {
        
        var postToReturnPublicDto = await _postService.GetPublic(permalink);
        return Ok(postToReturnPublicDto);
    }
    [HttpGet("my-content")]
    [Authorize]
    public async Task<ActionResult<List<PostToReturnForListDto>>> GetPost()
    {
        var authorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var list = await _postService.GetList(authorId);

        return Ok(list);
    }
    [HttpGet("public")]
    public async Task<ActionResult<List<PostToReturnForListPublicDto>>> GetPostPublic()
    {
        var list = await _postService.GetListPublic();
        return Ok(list);
    }

    [HttpPut("my-content/{id}")]
    [Authorize]
    public async Task<ActionResult<Post>> PutPost(PostToUpdateDto postToUpdate)
    {
        var authorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var postToReturnDto = await _postService.Update(postToUpdate, authorId);
        return Ok(postToReturnDto);
    }
    [HttpDelete("my-content/{id}")]
    [Authorize]
    public async Task<ActionResult<Post>> DeletePost(int id)
    {
        var authorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _postService.Delete(id, authorId);
        return Ok();
    }

    [Authorize]
    [HttpPost("my-content/upload-editor-image")]
    public async Task<JsonResult> UploadCkEditorImage()
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
        var previewPath = string.Empty;
        using (var memoryStream = new MemoryStream())
        {
            formFile.CopyTo(memoryStream);
            memoryStream.Position = 0;
            previewPath = await _imageService.UploadImageFireStore(memoryStream, upFileName, formFile.ContentType);
        }
        
        
        bool result = true;
        var rUpload = new
        {
            uploaded = result,
            url = result ? previewPath : string.Empty
        };
        return Json(rUpload);
    }
}