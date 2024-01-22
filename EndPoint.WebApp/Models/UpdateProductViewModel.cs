using Application.DTOs;
using Application.DTOs.ProductCategories;

namespace EndPoint.WebApp.Models
{
    public class UpdateProductViewModel
    {
        public ProductDetailsDto Product { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();
    }
}
