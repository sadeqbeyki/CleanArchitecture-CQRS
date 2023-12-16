using Application.DTOs;

namespace Application.Interface.Query
{
    public interface IProductQueryService
    {
        Task<ProductDetailsDto> GetById(int id);
    }
}
