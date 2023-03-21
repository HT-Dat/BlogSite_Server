using System.Security.Claims;
using FirebaseAdmin.Auth;
using System.Text.Json;
using BLL.Services.IServices;
using DTO.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpGet("verify-access")]
    [Authorize]
    public async Task<IActionResult> VerifyAccess()
    {
        string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var status = await _authService.VerifyAccess(uid);
        return Ok(status);
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> Register()
    {
        var userToRegisterDto = new UserToRegisterDto
        {
            id = User.FindFirst(ClaimTypes.NameIdentifier).Value,
            email = User.FindFirst(ClaimTypes.Email).Value
        };
        var user = await _authService.RegisterUser(userToRegisterDto);
        return Ok(user);
    }
}