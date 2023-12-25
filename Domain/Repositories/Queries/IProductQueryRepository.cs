using Domain.Entities.Products;
using Domain.Repositories.Queries.Base;

namespace Domain.Repositories.Queries
{
    public interface IProductQueryRepository: IQueryRepository<Product, Guid>
    {
    }
}
