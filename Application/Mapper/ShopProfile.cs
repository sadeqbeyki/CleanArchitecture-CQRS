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

        CreateMap<ProductCategory, ProductCategoryDto>();
    }
}
