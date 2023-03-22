using System.Security.Claims;
using BLL.Services.IServices;
using DAL.Entities;
using DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : Controller
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<ActionResult<Post>> PostPost()
    {
        var authorId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var a= await _postService.Add(authorId);

        return Ok(a);
    }
}