using Domain.Entities.Products;
using Domain.Interface.Queries;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.Query;

public class ProductQueryRepository : Repository<Product, Guid>, IProductQueryRepository
{
    private readonly ProductDbContext _dbContext;
    public ProductQueryRepository(ProductDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetProductsByName(string mailORphone)
    {
        var products = await _dbContext.Products
            .Where(p => p.ManufacturerPhone == mailORphone || p.ManufacturerEmail == mailORphone).ToListAsync();
        return products;
    }
}
