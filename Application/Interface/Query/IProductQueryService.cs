using Application.DTOs;
using Application.DTOs.ProductCategories;

namespace Application.Interface.Query;

public interface IProductQueryService
{
    Task<List<ProductDetailsDto>> GetProducts();
    Task<ProductDetailsDto> GetProductById(Guid id);
    Task<List<ProductDetailsDto>> GetProductsByEmail(string email);
    Task<List<ProductDetailsDto>> GetProductsByEmailPhone(string name);
}

public interface IProductCategoryQueryService
{
    Task<List<ProductCategoryDto>> GetProductCategories();

}
