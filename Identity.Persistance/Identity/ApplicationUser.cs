using Microsoft.AspNetCore.Identity;

namespace Identity.Persistance.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
}
