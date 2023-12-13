using Domain.Common;

namespace Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public string ManufacturePhone { get; private set; }
    public string ManufactureEmail { get; private set; }
    public bool IsAvailable { get; private set; }


    public Product(string name, string manufacturePhone, string manufactureEmail)
    {
        Name = name;
        ManufacturePhone = manufacturePhone;
        ManufactureEmail = manufactureEmail;
        IsAvailable = false;
    }
}
