using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Products;

public class Product : BaseEntity<Guid>
{
    [Required][StringLength(100)]
    public string Name { get; private set; }
    [Required][Phone]
    public string ManufacturerPhone { get; private set; }
    [Required(ErrorMessage ="need to choose")][EmailAddress]
    public string ManufacturerEmail { get; private set; }

    public bool IsAvailable { get; private set; }


    public Product(string name, string manufacturerPhone, string manufacturerEmail)
    {
        Name = name;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
        IsAvailable = false;
    }
    public void Edit(string name, string manufacturerPhone, string manufacturerEmail)
    {
        Name = name;
        ManufacturerPhone = manufacturerPhone;
        ManufacturerEmail = manufacturerEmail;
    }
}
