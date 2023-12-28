using Application.DTOs;

namespace Application.Interface.Command;

public interface IProductCommandService
{
    Task<ProductDetailsDto> AddProduct(AddProductDto product);

}
