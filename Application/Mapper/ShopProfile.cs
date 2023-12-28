using Application.DTOs;
using AutoMapper;
using Domain.Entities.Products;

namespace Application.Mapper;

public class ShopProfile : Profile
{
    public ShopProfile()
    {
        CreateMap<Product, ProductDetailsDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
    }
}
