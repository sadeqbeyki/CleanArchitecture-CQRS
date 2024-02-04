using Identity.Persistance.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistance.DatabaseMapping;

public class UserMapping : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        //var Id = Guid.NewGuid().ToString();
        //var stamp = Guid.NewGuid().ToString();
        //builder.HasData(
        //            new ApplicationUser
        //            {
        //                Id = Id,
        //                UserName = "admin",
        //                NormalizedUserName = "ADMIN",
        //                Email = "admin@example.com",
        //                NormalizedEmail = "ADMIN@EXAMPLE.COM",
        //                EmailConfirmed = true,
        //                PasswordHash = "AQAAAAEAACcQAAAAEOo4nGyVl3KU2bqMIU0fWPrS5J8sZQp2JYjUsfBJ/xmS0gd48EDqlhEEdIK34bRg3Q==", // Replace with hashed password
        //                SecurityStamp = "NHY6B5L2Z5I3CLAZOTGTRKTRHK6NKMRD",
        //                ConcurrencyStamp = stamp,
        //            });
    }
}

