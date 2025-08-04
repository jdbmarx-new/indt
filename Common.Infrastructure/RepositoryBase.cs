using Common.Domain;
using Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Data.EFCore
{
    public abstract class RepositoryBase<T, TContext> : IRepository<T> where T : Entity where TContext : DbContext
    {
        protected TContext Context { get; }
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(TContext context)
        {
            Context = context;
            _dbSet = Context.Set<T>();
        }

        public Task<T?> GetByIdAsync(int id)
        {
            return _dbSet.FindAsync(id).AsTask()!;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync().ConfigureAwait(false);
        }

        public Task AddAsync(T entity)
        {
            return _dbSet.AddAsync(entity).AsTask();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}