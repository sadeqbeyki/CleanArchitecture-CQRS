using Application.DTOs;
using AutoMapper;
using Domain.Entities.Products;
using Identity.Application.Common.Enums;
using Identity.Application.DTOs;
using Identity.Application.DTOs.Auth;
using Identity.Persistance.Identity;

namespace Application.Mapper
{
    public class ShopProfile : Profile
    {
        public ShopProfile()
        {
            CreateMap<ProductDetailsDto, Product>();
            CreateMap<Product, ProductDetailsDto>();
        }
    }
}
