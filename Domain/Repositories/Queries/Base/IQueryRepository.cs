using Domain.Entities;

namespace Domain.Repositories.Queries.Base
{
    public interface IQueryRepository <TEntity, TKey> where TEntity : BaseEntity
    {
        Task<TEntity> GetByIdAsync(TKey id);
    }
}
