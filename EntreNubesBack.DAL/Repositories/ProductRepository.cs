using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{

    public ProductRepository(EntrenubesContext dbContext) : base(dbContext)
    {
        
    }
}