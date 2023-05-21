using AutoMapper;
using AutoMapper.QueryableExtensions;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.BLL.Services;

public class RolService : IRolService
{
    private readonly IRolRepository _rolRepository;
    private readonly IGenericRepository<Action> _actionsRepository;
    private readonly IMapper _mapper;

    public RolService(IRolRepository rolRepository, IMapper mapper, IGenericRepository<Action> actionsRepository)
    {
        _rolRepository = rolRepository;
        _mapper = mapper;
        _actionsRepository = actionsRepository;
    }

    public async Task<List<RolDto>> List()
    {
        try
        {
            var rolesQuery = await _rolRepository.Consult(r => r.State);
            var roles = rolesQuery.Include(r => r.IdActions);
            return _mapper.Map<List<RolDto>>(roles);
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<ActionDto>> Actions()
    {
        try
        {
            var rolesQuery = await _actionsRepository.ConsultAsNoTacking();
            return _mapper.Map<List<ActionDto>>(rolesQuery);
        }
        catch
        {
            throw;
        }
    }

    public async Task<RolDto> Create(CreateRolDto newRolInfo)
    {
        try
        {
            var rol = await _rolRepository.Get(r => r.State && r.RolName.ToLower() == newRolInfo.Name.ToLower());
            if (rol != null)
                throw new TaskCanceledException("Nombre de rol ya existente.");
            
            var newRol = await _rolRepository.CreateRol(newRolInfo.Name, newRolInfo.Actions);
            if (newRol == null)
                throw new TaskCanceledException("No se pudo crear el rol.");
            return _mapper.Map<RolDto>(newRol);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> Edit(EditRolDto rolDto)
    {
        try
        {
            var actualRol =  _rolRepository.Consult(r => r.IdRol == rolDto.IdRol && r.State).Result.Include(a => a.IdActions).FirstOrDefault();
            if (actualRol == null)
                throw new TaskCanceledException("El rol no existe");

            actualRol.RolName = rolDto.Name;
            actualRol.IdActions.Clear();
            foreach (int idAction in rolDto.Actions)
            {
                var action = await _actionsRepository.Get(a => a.IdAction == idAction);
                actualRol.IdActions.Add(action);
            }

            bool response = await _rolRepository.Edit(actualRol);
            if (!response)
                throw new TaskCanceledException("No se pudo editar el rol");
            return response;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> Delete(int rolId)
    {
        try
        {
            var rol = await _rolRepository.Get(r => r.IdRol == rolId);
            if (rol == null)
                throw new TaskCanceledException("El rol no existe");
            rol.State = false;
            bool result = await _rolRepository.Edit(rol);
            if (!result)
                throw new TaskCanceledException("Error al eliminar el rol");
            return result; 
        }
        catch
        {
            throw;
        }
    }
}