using Identity.Persistance.DatabaseMapping;
using Identity.Persistance.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistance;

public class AppIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
    {
    }


    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    var assembly = typeof(RoleMapping).Assembly;
    //    modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    //    base.OnModelCreating(modelBuilder);
    //}

}
