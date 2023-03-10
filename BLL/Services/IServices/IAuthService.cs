using DTO.DTOs;

namespace BLL.Services.IServices;

public interface IAuthService
{
    Task<UserRegistrationStatusReturnDto> VerifyAccess(string uid);
}