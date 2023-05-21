using EntreNubesBack.DTO;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IUserService
{
    Task<List<UserDto>> List();
    Task<UserDto> Create(CreateUserDto user);
    Task<bool> Edit(EditUserDto user);
    Task<bool> ChangeStatus(int userId);
}