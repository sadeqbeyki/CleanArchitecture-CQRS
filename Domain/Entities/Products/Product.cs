using Domain.Entities.BookCategoryAgg;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Products;

public class Product : BaseEntity<Guid>
{
    [Required(ErrorMessage = "need to choose")]
    [StringLength(50)]
    public string Name { get; private set; }
    public string MemberId { get; set; }

    [Phone]
    public string ManufacturerPhone { get; private set; }

    [EmailAddress]
    public string ManufacturerEmail { get; private set; }


    public bool IsAvailable { get; private set; }

    public ProductCategory Category { get; set; }
    public int CategoryId { get; set; }

    public Product(string name, string memberId, string manufacturerPhone, string manufacturerEmail, int categoryId)
    {
        Name = name;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
        MemberId = memberId;
        CategoryId = categoryId;
        IsAvailable = false;
    }
    public void Edit(string name, string memberId, string manufacturerPhone, string manufacturerEmail, int categoryId)
    {
        Name = name;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
        MemberId = memberId;
        CategoryId = categoryId;
    }
}
