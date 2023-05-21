using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories;

public class ThirdPartyRepository : GenericRepository<ThirdParty>, IThirdPartyRepository
{
    public ThirdPartyRepository(EntrenubesContext dbContext) : base(dbContext)
    {
        
    }
}