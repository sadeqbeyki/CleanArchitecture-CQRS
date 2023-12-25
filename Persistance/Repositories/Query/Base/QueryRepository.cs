using Domain.Entities;
using Domain.Repositories.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.Query.Base
{
    public class QueryRepository<TEntity, TKey> : IQueryRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public QueryRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            var result = await _dbSet.FindAsync(id)
                ?? throw new Exception($"Entity with id {id} not found.");
            return result;

        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
