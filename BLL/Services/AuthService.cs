using BLL.Services.IServices;
using DAL;
using DTO.DTOs;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private readonly string REGISTERED_STATUS = "registered";
    private readonly string UNREGISTERED_STATUS = "unregistered";
    private readonly BlogSiteDbContext _blogSiteDbContext;

    public AuthService(BlogSiteDbContext blogSiteDbContext)
    {
        _blogSiteDbContext = blogSiteDbContext;
    }

    public async Task<UserRegistrationStatusReturnDto> VerifyAccess(string uid)
    {
        string message;
        string status;
        if (_blogSiteDbContext.Users.Any(o => o.Id == uid))
        {
            status = REGISTERED_STATUS;
            message = "The current user is registered";

        }
        else
        {
            status = UNREGISTERED_STATUS;
            message = "The current user is not registered";
        }
        return new UserRegistrationStatusReturnDto
        {
            Message = message,
            Status = status
        };
    }
}