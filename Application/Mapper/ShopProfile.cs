using Application.DTOs;
using Application.DTOs.ProductCategories;
using AutoMapper;
using Domain.Entities.BookCategoryAgg;
using Domain.Entities.Products;

namespace Application.Mapper;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<Product, ProductDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        // Mapping from ProductDetailsDto to UpdateProductDto
        CreateMap<ProductDetailsDto, UpdateProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ReverseMap(); // Add ReverseMap for two-way mapping

        //// Mapping from UpdateProductDto to Product
        //CreateMap<UpdateProductDto, Product>()
        //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));


        CreateMap<ProductCategory, ProductCategoryDto>();
    }
}
