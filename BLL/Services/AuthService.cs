using BLL.Services.IServices;
using BLL.Utilities;
using DAL.Persistence;
using DAL.Entities;
using DTO.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class AuthService : IAuthService
{
    private readonly string REGISTERED_STATUS = "registered";
    private readonly string UNREGISTERED_STATUS = "unregistered";
    private readonly IBlogSiteDbContext _blogSiteDbContext;
    private readonly ISystemClock _systemClock;
    public AuthService(IBlogSiteDbContext blogSiteDbContext, ISystemClock systemClock)
    {
        _blogSiteDbContext = blogSiteDbContext;
        _systemClock = systemClock;
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
        var userInDb = await _blogSiteDbContext.Users.FindAsync(userToRegisterDto.Id);
        if (userInDb != null)
        {
            userInDb.LastLogin = _systemClock.UtcNow;
            userInDb.PhotoUrl = userToRegisterDto.PhotoUrl;
            _blogSiteDbContext.SetModified(userInDb);
            await _blogSiteDbContext.SaveChangesAsync();
            return userInDb;
        }
        var addingUser = new User
        {
            Id = userToRegisterDto.Id,
            Email = userToRegisterDto.Email,
            DisplayName = userToRegisterDto.DisplayName,
            Intro = string.Empty,
            Profile = string.Empty,
            SexId = 0,
            CreatedDate = DateTime.UtcNow,
            LastLogin = DateTime.Now,
            PhotoUrl = userToRegisterDto.PhotoUrl
        };
        await _blogSiteDbContext.Users.AddAsync(addingUser);
        await _blogSiteDbContext.SaveChangesAsync();
        return addingUser;
        
    }
}