using Application.DTOs;

namespace Application.Interface.Query;

public interface IProductQueryService
{
    List<ProductDetailsDto> GetProducts();
    Task<ProductDetailsDto> GetProductById(Guid id);
    List<ProductDetailsDto> GetProductsByEmail(string email);
    Task<List<ProductDetailsDto>> GetProductsByEmailPhone(string name);
}
