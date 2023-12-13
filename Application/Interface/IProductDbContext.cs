using Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace Application.Interface;

public interface IProductDbContext
{
    DbSet<Product> Products { get; set; }
    Task<int> SaveChangeAsync();
}
