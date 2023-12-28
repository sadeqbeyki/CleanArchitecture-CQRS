using Application.DTOs;
using Application.Interface.Command;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Interface;
using Infrastructure.ACL;
using MediatR;

namespace Services.Command;

public class ProductCommandService : IProductCommandService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IMapper _mapper;

    private readonly IUserServiceACL _userServiceACL;


    public ProductCommandService(IUnitOfWork unitOfWork, IMapper mapper, IUserServiceACL userServiceACL)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _productRepository = _unitOfWork.GetRepository<Product, Guid>();

        _userServiceACL = userServiceACL;
    }
    public async Task<ProductDetailsDto> AddProduct(AddProductDto dto)
    {
        var user = await _userServiceACL.GetCurrentUser();

        var product = new Product(dto.Name, user.PhoneNumber, user.Email);
        var newProduct = await _productRepository.CreateAsync(product);
        _productRepository.SaveChanges();

        var mapProduct = _mapper.Map<ProductDetailsDto>(newProduct);
        return mapProduct;
    }
}
