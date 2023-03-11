using System.Security.Claims;
using FirebaseAdmin.Auth;
using System.Text.Json;
using BLL.Services.IServices;
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
    [HttpPost("verify-access")]
    [Authorize]
    public async Task<IActionResult> VerifyAccess()
    {
        string uid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        Console.WriteLine(uid);
        var status = await _authService.VerifyAccess(uid);
        return Ok(status);
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> Register()
    {
        
        return Ok();
    }
}