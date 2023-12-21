using AutoMapper;
using Identity.Application.Common.Enums;
using Identity.Application.DTOs;
using Identity.Application.DTOs.Auth;
using Identity.Persistance.Identity;

namespace Identity.Application.Mapper
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<ApplicationUser, UserDetailsResponseDto>();
            CreateMap<ApplicationUser, UserDetailsDto>()
                .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Culture, options => options.MapFrom(src => (SupportedCulture)src.Culture));

            CreateMap<RegisterUserDto, ApplicationUser>()
                .BeforeMap((src, dest) =>
                {
                    dest.JoinedOn = DateTime.Now;
                    //dest.UserName = src.Email;
                    dest.Culture = (int)SupportedCulture.fa;
                });
        }
    }
}
