using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories;

public class SaleRepository : GenericRepository<Sale>, ISaleRepository
{
    public SaleRepository(EntrenubesContext dbContext) : base(dbContext)
    {
        
    }
}