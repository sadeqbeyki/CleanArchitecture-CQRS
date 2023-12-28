using Domain.Entities.Products;
namespace Domain.Interface.Queries;

public interface IProductQueryRepository : IRepository<Product, Guid>
{
}
