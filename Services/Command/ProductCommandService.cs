﻿using Application.DTOs;
using Application.Exceptions;
using Application.Interface.Command;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Interface;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.ACL;
using Microsoft.AspNetCore.Http;

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
        ValidationResult validationResult = await _validator.ValidateAsync(dto);

        string filePath = UploadImage(dto.Image);

        if (!validationResult.IsValid)
        {
            foreach (var failure in validationResult.Errors)
            {
                throw new Exception("Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage);
            }
        }
        else
        {
            //var user = await _userServiceACL.GetCurrentUser();
            Product product = new(
                dto.Name,
                //user.Id,
                //user.PhoneNumber,
                //user.Email,
                dto.MemberId,
                dto.ManufacturerPhone,
                dto.ManufacturerEmail,
                filePath,
                dto.CategoryId);
            var newProduct = await _productRepository.CreateAsync(product);

            var mapProduct = _mapper.Map<ProductDetailsDto>(newProduct);
            return mapProduct;
        }

        throw new Exception(" cant add new product " + validationResult.Errors.ToList());
    }



    public async Task<Guid> DeleteProduct(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId)
             ?? throw new NotFoundException(" product not found !");

        //var user = await _userServiceACL.GetCurrentUser()
        //    ?? throw new NotFoundException(" user not found !");

        //if (product.ManufacturerEmail != user.Email)
        //{
        //    throw new BadRequestException("You can only delete products that you have created yourself");
        //}

        var result = _productRepository.DeleteAsync(product);
        return product.Id;
    }

    public async Task<Guid> UpdateProduct(UpdateProductDto dto)
    {
        //var user = await _userServiceACL.GetCurrentUser()
        //?? throw new NotFoundException(" user not found !");

        var product = await _productRepository.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException(" product not found !");

        //if (user.Email != product.ManufacturerEmail)
        //    throw new BadRequestException(" You can only edit products that you have created yourself. ");

        string filePath = dto.Image != null ? UploadImage(dto.Image) : product.ImageUrl;

        product.Edit(
            dto.Name,
            //user.Id,
            //user.PhoneNumber,
            //user.Email,
            dto.MemberId,
            dto.ManufacturerPhone,
            dto.ManufacturerEmail,
            filePath,
            dto.CategoryId);
        await _productRepository.UpdateAsync(product);

        return product.Id;
    }

    private static string UploadImage(IFormFile image)
    {
        Guid guid = Guid.NewGuid();
        var filePath = Path.Combine("wwwroot/ProductImages/", guid + ".jpg");
        if (image != null)
        {
            var fileStream = new FileStream(filePath, FileMode.Create);
            image.CopyTo(fileStream);
        }

        return filePath.Remove(0, 7);
    }
}
