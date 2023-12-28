using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interface;

public interface IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<TEntity> GetByIdAsync(TKey id);
    IList<TEntity> GetAll();

    Task<TEntity> CreateAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);

    bool Exists(Expression<Func<TEntity, bool>> expression);
    void SaveChanges();
    void Dispose();
}
