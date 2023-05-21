using System.Linq.Expressions;

namespace EntreNubesBack.DAL.Repositories.Contracts
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<TModel> Get(Expression<Func<TModel, bool>> filter);
        Task<TModel> Create(TModel model);
        Task<bool> Edit(TModel model);
        Task<bool> Delete(TModel model);
        Task<IQueryable<TModel>> Consult(Expression<Func<TModel, bool>> filter = null);
        Task<IQueryable<TModel>> ConsultAsNoTacking(Expression<Func<TModel, bool>> filter = null);
        Task<IQueryable<TModel>> GetAll();
    }
}
