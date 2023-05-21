using EntreNubesBack.Models;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.DAL.Repositories.Contracts;

public interface IRolRepository : IGenericRepository<Role>
{
    Task<Role> CreateRol(string rolName, List<int> actions);
}