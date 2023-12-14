using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.User.Queries
{
    public class GetUserDetailsQuery:IRequest<UserDetailsResponseDto>
    {
        public string UserId { get; set; }
    }
}
