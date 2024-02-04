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

        List<ApplicationRole> applicationRoles = new(){
                        new(){Name = "Member"},
                        new(){Name = "Manager"},
                        new(){Name = "Employee"},
                        new(){Name = "Admin"}
                    };

        builder.HasData(applicationRoles);
    }
}
