using Application.DTOs;

namespace Application.Interface.Query
{
    public interface IProductQueryService
    {
        Task<ProductDetailsDto> GetProductById(int id);
    }
}
