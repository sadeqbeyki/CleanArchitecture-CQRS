using Identity.Persistance.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Persistance.DatabaseMapping;

public class RoleMapping : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        //builder.HasKey(x => x.Id);
        //builder.ToTable("Roles");

        builder.HasData(
            new { Id = "1", Name = "Member", NormalizedName = "MEMBER" },
            new { Id = "2", Name = "Manager", NormalizedName = "MANAGER" },
            new { Id = "3", Name = "Employee", NormalizedName = "EMPLOYEE" },
            new { Id = "4", Name = "Admin", NormalizedName = "ADMIN" });
    }
}

