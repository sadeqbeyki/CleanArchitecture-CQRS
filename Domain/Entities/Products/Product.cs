using Domain.Entities.BookCategoryAgg;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Products;

public class Product : BaseEntity<Guid>
{
    public string Name { get; private set; }
    public string MemberId { get; set; }
    public string ManufacturerPhone { get; private set; }
    public string ManufacturerEmail { get; private set; }
    public string ImageUrl { get; set; }

    public bool IsAvailable { get; private set; }

    public ProductCategory Category { get; set; }
    public int CategoryId { get; set; }


    public Product(string name, string memberId, string manufacturerPhone, string manufacturerEmail, string imageUrl, int categoryId)
    {
        Name = name;
        MemberId = memberId;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
        IsAvailable = false;
    }
    public void Edit(string name, string memberId, string manufacturerPhone, string manufacturerEmail, string imageUrl, int categoryId)
    {
        Name = name;
        MemberId = memberId;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
    }
}
