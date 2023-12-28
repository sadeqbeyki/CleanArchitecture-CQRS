using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interface;

namespace Persistance.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductDbContext _context;
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(ProductDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Rollback()
    {
        // Rollback changes if needed
    }

    public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IRepository<TEntity, TKey>)_repositories[typeof(TEntity)];
        }

        var repository = new Repository<TEntity, TKey>(_context);
        _repositories.Add(typeof(TEntity), repository);
        return repository;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
