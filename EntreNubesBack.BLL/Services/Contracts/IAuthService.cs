using EntreNubesBack.DTO;
using EntreNubesBack.Models;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IAuthService
{
    Task<SessionDto> ValidateCredentials(string email, string password);
}