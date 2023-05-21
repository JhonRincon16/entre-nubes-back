using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.Models;
using Action = EntreNubesBack.Models.Action;

namespace EntreNubesBack.DAL.Repositories;

public class RolRepository : GenericRepository<Role>, IRolRepository
{
    private readonly IGenericRepository<Action> _actionRepository;

    public RolRepository(EntrenubesContext dbContext, IGenericRepository<Action> actionRepository) : base(dbContext)
    {
        _actionRepository = actionRepository;
    }
    
    public async Task<Role> CreateRol(string rolName, List<int> actions)
    {
        var rol = new Role();
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                Role newRol = new Role() { State = true, RolName = rolName};
                foreach (int idAction in actions)
                {
                    var actualAction = await _actionRepository.Get(a => a.IdAction == idAction);
                    newRol.IdActions.Add(actualAction);
                }
                var newRole = await _dbContext.Roles.AddAsync(newRol);
                await _dbContext.SaveChangesAsync();
                
                transaction.Commit();
                rol = await Get(r => r.IdRol == newRole.Entity.IdRol);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            return rol;
        }
    }
}