using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public abstract class ProductDto
{
    [Required(ErrorMessage = "Name field is required!")]
    [StringLength(50)]
    public string Name { get; set; }

    public string MemberId { get; set; }

    [Phone]
    public string ManufacturerPhone { get; set; }

    [EmailAddress]
    public string ManufacturerEmail { get; set; }

    [Required]
    public int CategoryId { get; set; }
}
