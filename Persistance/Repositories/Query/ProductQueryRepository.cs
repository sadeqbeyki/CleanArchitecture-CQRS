using Domain.Entities.Products;
using Domain.Interface.Queries;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.Query;

public class ProductQueryRepository : Repository<Product, Guid>, IProductQueryRepository
{
    public ProductQueryRepository(ProductDbContext dbContext) : base(dbContext)
    {
    }
}
