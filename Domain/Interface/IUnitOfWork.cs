using Domain.Entities;

namespace Domain.Interface;

public interface IUnitOfWork : IDisposable
{
    void Commit();
    void Rollback();
    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
}
