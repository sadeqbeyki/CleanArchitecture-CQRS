using Application.DTOs;

namespace Application.Interface.Query
{
    public interface IProductQueryService
    {
        Task<ProductDetailsDto> GetProductById(Guid id);
    }
}
