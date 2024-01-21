using Application.DTOs.ProductCategories;

namespace Application.Interface.Query;

public interface IProductCategoryQueryService
{
    Task<List<ProductCategoryDto>> GetProductCategories();

}
