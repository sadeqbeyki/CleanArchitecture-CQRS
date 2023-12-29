using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interface;
using System.Linq.Expressions;

namespace Persistance.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public Repository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(TKey id)
    {
        var result = await _dbSet.FindAsync(id);
        if (result == null)
        {
            throw new Exception($"Entity with id {id} not found.");
        }
        return result;
    }
    public IList<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>().ToList();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public bool Exists(Expression<Func<TEntity, bool>> expression)
    {
        return _dbContext.Set<TEntity>().Any(expression);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
