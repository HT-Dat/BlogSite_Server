using BLL.Services.IServices;
using DAL.Persistence;
using DAL.Entities;
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

    public async Task<User> RegisterUser(UserToRegisterDto userToRegisterDto)
    {
        var userInDb = await _blogSiteDbContext.Users.FindAsync(userToRegisterDto.id);
        if (userInDb != null)
        {
            userInDb.LastLogin = DateTime.UtcNow;
            await _blogSiteDbContext.SaveChangesAsync();
            return userInDb;
        }
        var addingUser = new User
        {
            Id = userToRegisterDto.id,
            Email = userToRegisterDto.email,
            DisplayName = string.Empty,
            Intro = string.Empty,
            Profile = string.Empty,
            SexId = 1,
            CreatedDate = DateTime.UtcNow,
            LastLogin = DateTime.Now
        };
        await _blogSiteDbContext.Users.AddAsync(addingUser);
        await _blogSiteDbContext.SaveChangesAsync();
        return addingUser;

    }
}