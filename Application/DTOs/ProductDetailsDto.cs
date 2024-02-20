using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class ProductDetailsDto : ProductDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsAvailable { get; set; }
    public string Category { get; set; }
    public string ImageUrl { get; set; }
}

public class AddProductDto : ProductDto
{
    public IFormFile Image { get; set; }
}
public class UpdateProductDto : ProductDto
{
    public Guid Id { get; set; }
    public IFormFile Image { get; set; }
}
