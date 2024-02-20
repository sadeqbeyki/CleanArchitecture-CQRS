using Microsoft.AspNetCore.Identity;
using Domain.Entities.Products;

namespace Identity.Persistance.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = String.Empty;


    public int Culture { get; set; }
    public DateTime? JoinedOn { get; set; }

    public ICollection<Product> Products { get; set; }
}
