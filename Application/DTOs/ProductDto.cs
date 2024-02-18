
namespace Application.DTOs;

public abstract class ProductDto
{
    public string Name { get; set; }
    public string MemberId { get; set; }
    public string ManufacturerPhone { get; set; }
    public string ManufacturerEmail { get; set; }
    public int CategoryId { get; set; }
}
