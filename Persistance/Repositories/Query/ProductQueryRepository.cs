using Domain.Entities.Products;
using Domain.Interface.Queries;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.Query;

public class ProductQueryRepository : Repository<Product, Guid>, IProductQueryRepository
{
    private readonly ProductDbContext _productDbContext;
    public ProductQueryRepository(ProductDbContext dbContext, ProductDbContext productDbContext) : base(dbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<List<Product>> GetProductByEmail(string email)
    {
        var products = await _productDbContext.Products.Where(e => e.ManufacturerEmail == email).ToListAsync();
        return products;
    }
}
