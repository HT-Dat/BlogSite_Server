using BLL.Services.IServices;
using DAL;
using DTO.DTOs;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private BlogSiteDbContext _blogSiteDbContext;
    public async Task<UserRegistrationStatusReturnDto> VerifyAccess(string uid)
    {
        return new UserRegistrationStatusReturnDto
        {
            Message = "abc",
            Status = "cde"
        };
    }
}