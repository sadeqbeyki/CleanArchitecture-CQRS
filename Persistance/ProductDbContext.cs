using Application.Interface;
using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistance;

public class ProductDbContext : DbContext, IProductDbContext
{
    public DbSet<Product> Products { get; set; }

    public ProductDbContext(DbContextOptions options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(ProductConfigurations).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> SaveChangeAsync()
    {
        return await base.SaveChangesAsync();
    }
}
