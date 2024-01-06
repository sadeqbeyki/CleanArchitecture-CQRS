using Domain.Entities.Products;

namespace Domain.Entities.BookCategoryAgg;

public class ProductCategory : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Product> Products { get; set; }

    public ProductCategory(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
