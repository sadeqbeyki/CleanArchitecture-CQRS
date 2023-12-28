using Application.DTOs;

namespace Application.Interface.Query;

public interface IProductQueryService
{
    Task<ProductDetailsDto> GetProductById(Guid id);
    List<ProductDetailsDto> GetProducts();
    List<ProductDetailsDto> GetProductsByEmail(string email);
}
