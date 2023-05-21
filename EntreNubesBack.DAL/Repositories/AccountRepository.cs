using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories;

public class AccountRepository : GenericRepository<Account>, IAccountRepository
{
    public AccountRepository(EntrenubesContext dbContext) : base(dbContext)
    {
    }
}