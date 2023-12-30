using Application.DTOs;
using System.Runtime.InteropServices;

namespace Application.Interface.Command;

public interface IProductCommandService
{
    Task<ProductDetailsDto> AddProduct(AddProductDto product);
    Task<Guid> UpdateProduct(UpdateProductDto dto);
    Task<Guid> DeleteProduct(Guid productId);

}
