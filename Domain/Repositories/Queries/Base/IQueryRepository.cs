using Domain.Entities;

namespace Domain.Repositories.Queries.Base
{
    public interface IQueryRepository <TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id);
    }
}
