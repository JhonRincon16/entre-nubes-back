using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EntreNubesBack.Models;

namespace EntreNubesBack.DAL.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel>
        where TModel : class
    {
        private readonly DbSet<TModel> DbSet;
        private readonly DbSet<HistoryChange> HistoryChangeSet;
        protected readonly EntrenubesContext _dbContext;

        public GenericRepository(EntrenubesContext dbContext)
        {
            _dbContext = dbContext;
            DbSet = _dbContext.Set<TModel>();
            HistoryChangeSet = _dbContext.Set<HistoryChange>();
        }

        public async Task<TModel> Get(Expression<Func<TModel, bool>> filter)
        {
            try
            {
                TModel model = await DbSet.FirstOrDefaultAsync(filter);
                return model;
            } catch
            {
                throw;
            }
        }

        public async Task<TModel> Create(TModel model)
        {
            try
            {
                DbSet.Add(model);
                HistoryChangeSet.Add(new HistoryChange()
                {
                    Actions = "Create",
                    Date = DateTime.Now.Date,
                    TableName = DbSet.EntityType.Name
                });
                await _dbContext.SaveChangesAsync();
                return model;
            } catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(TModel model)
        {
            try
            {
                DbSet.Update(model);
                HistoryChangeSet.Add(new HistoryChange()
                {
                    Actions = "Edit",
                    Date = DateTime.Now.Date,
                    TableName = DbSet.EntityType.Name
                });
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(TModel model)
        {
            try
            {
                DbSet.Remove(model);
                HistoryChangeSet.Add(new HistoryChange()
                {
                    Actions = "Delete",
                    Date = DateTime.Now.Date,
                    TableName = DbSet.EntityType.Name
                });
                await _dbContext.SaveChangesAsync();
                return true;
            } catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModel>> Consult(Expression<Func<TModel, bool>> filter = null)
        {
            try
            {
                IQueryable<TModel> queryModel = filter == null ? DbSet : DbSet.Where(filter);
                DbSet.Where(filter).AsNoTracking();
                return queryModel;
            } catch
            {
                throw;
            }
        }

        public async Task<IQueryable<TModel>> ConsultAsNoTacking(Expression<Func<TModel, bool>> filter = null)
        {
            try
            {
                IQueryable<TModel> queryModel = filter == null ? DbSet.AsNoTracking() : DbSet.Where(filter).AsNoTracking();
                return queryModel;
            } catch
            {
                throw;
            }
        } 

        public Task<IQueryable<TModel>> GetAll()
        {
            return null;
        }
    }
}
