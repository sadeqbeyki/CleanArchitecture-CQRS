using Domain.Entities;
using Domain.Repositories.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.Query.Base
{
    public class QueryRepository<TEntity,TKey> : IQueryRepository<TEntity, TKey> where TEntity : BaseEntity
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
            return await _dbSet.FindAsync(id);

        }
    }
}
