using Domain.Entities.BookCategoryAgg;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Products;

public class Product : BaseEntity<Guid>
{
    [Required]
    [StringLength(100)]
    public string Name { get; private set; }

    [Required]
    [Phone]
    public string ManufacturerPhone { get; private set; }

    [Required(ErrorMessage = "need to choose")]
    [EmailAddress]
    public string ManufacturerEmail { get; private set; }

    public bool IsAvailable { get; private set; }

    public ProductCategory Category { get; set; }
    public int CategoryId { get; set; }

    public Product(string name, string manufacturerPhone, string manufacturerEmail, int categoryId)
    {
        Name = name;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
        CategoryId = categoryId;
        IsAvailable = false;
    }
    public void Edit(string name, string manufacturerPhone, string manufacturerEmail, int categoryId)
    {
        Name = name;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
        CategoryId = categoryId;
    }
}
