using Domain.Entities.Products;
using Domain.Repositories.Queries;
using Microsoft.EntityFrameworkCore;
using Persistance.Repositories.Query.Base;

namespace Persistance.Repositories.Query
{
    public class ProductQueryRepository : QueryRepository<Product, int>, IProductQueryRepository
    {
        public ProductQueryRepository(ProductDbContext dbContext) : base(dbContext)
        {
        }
    }
}
