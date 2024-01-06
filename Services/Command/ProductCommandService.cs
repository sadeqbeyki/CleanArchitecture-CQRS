using Application.DTOs;
using Application.Exceptions;
using Application.Interface.Command;
using Application.Validation.Customers;
using Application.Validation.Products;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Interface;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.ACL;


namespace Services.Command;

public class ProductCommandService : IProductCommandService
{
    private readonly IValidator<AddProductDto> _validator;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IMapper _mapper;

    private readonly IUserServiceACL _userServiceACL;


    public ProductCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUserServiceACL userServiceACL,
        IValidator<AddProductDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _productRepository = _unitOfWork.GetRepository<Product, Guid>();

        _userServiceACL = userServiceACL;
        _validator = validator;
    }
    public async Task<ProductDetailsDto> AddProduct(AddProductDto dto)
    {
        ProductValidator validator = new();
        ValidationResult results = await _validator.ValidateAsync(dto);
        if (!results.IsValid)
        {
            foreach (var failure in results.Errors)
            {
                Console.WriteLine("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            }
        }
        var user = await _userServiceACL.GetCurrentUser();
        var product = new Product(dto.Name, user.PhoneNumber, user.Email, dto.categoryId);
        var newProduct = await _productRepository.CreateAsync(product);

        var mapProduct = _mapper.Map<ProductDetailsDto>(newProduct);
        return mapProduct;
    }

    public async Task<Guid> DeleteProduct(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId)
             ?? throw new NotFoundException(" product not found !");

        var user = await _userServiceACL.GetCurrentUser()
            ?? throw new NotFoundException(" user not found !");

        if (product.ManufacturerEmail != user.Email)
        {
            throw new BadRequestException("You can only delete products that you have created yourself");
        }

        var result = _productRepository.DeleteAsync(product);
        return product.Id;
    }

    public async Task<Guid> UpdateProduct(UpdateProductDto dto)
    {
        var user = await _userServiceACL.GetCurrentUser()
            ?? throw new NotFoundException(" user not found !");

        var existProduct = await _productRepository.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException(" product not found !");

        if (user.Email != existProduct.ManufacturerEmail)
        {
            throw new BadRequestException(" You can only edit products that you have created yourself. ");
        }

        existProduct.Edit(dto.Name, user.PhoneNumber, user.Email);
        await _productRepository.UpdateAsync(existProduct);

        return existProduct.Id;
    }
}
