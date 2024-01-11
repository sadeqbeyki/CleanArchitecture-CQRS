using Domain.Entities.BookCategoryAgg;
using Domain.Entities.CustomerAgg;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistance;

public interface IProductDbContext
{
    DbSet<Product> Products { get; set; }
    DbSet<ProductCategory> ProductCategories { get; set; }
    DbSet<Customer> Customers { get; set; }
}

public class ProductDbContext : DbContext, IProductDbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Customer> Customers { get; set; }

    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(ProductCategoryConfigurations).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}
