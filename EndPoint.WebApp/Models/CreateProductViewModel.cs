using Application.DTOs;
using Application.DTOs.ProductCategories;

namespace EndPoint.WebApp.Models;

public class CreateProductViewModel
{
    public string Name { get; set; }
    public string ManufacturerPhone { get; set; }
    public string ManufacturerEmail { get; set; }
    public int CategoryId { get; set; }

    public List<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();
}
