using EntreNubesBack.DTO;
using EntreNubesBack.Models;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IRolService
{
    Task<List<RolDto>> List();
    Task<List<ActionDto>> Actions();
    Task<RolDto> Create(CreateRolDto newRolInfo);
    Task<bool> Edit(EditRolDto rolDto);
    Task<bool> Delete(int rolId);
}