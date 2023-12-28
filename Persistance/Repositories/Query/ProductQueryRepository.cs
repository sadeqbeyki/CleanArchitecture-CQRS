using Domain.Entities.Products;
using Domain.Interface.Queries;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.Query;

public class ProductQueryRepository : Repository<Product, Guid>, IProductQueryRepository
{
    //private readonly ProductDbContext _shopContext;
    public ProductQueryRepository(ProductDbContext dbContext) : base(dbContext)
    {
        //_shopContext = dbContext;
    }
}
