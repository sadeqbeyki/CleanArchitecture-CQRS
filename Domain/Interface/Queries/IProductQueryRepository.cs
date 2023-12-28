using Domain.Entities.Products;
namespace Domain.Interface.Queries;

public interface IProductQueryRepository : IRepository<Product, Guid>
{
    Task<List<Product>> GetProductByEmail(string email);
}
