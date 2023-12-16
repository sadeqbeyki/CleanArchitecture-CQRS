using Domain.Common;

namespace Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public string ManufacturerPhone { get; private set; }
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
