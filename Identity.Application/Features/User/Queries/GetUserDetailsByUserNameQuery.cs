using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.User.Queries
{
    public class GetUserDetailsByUserNameQuery : IRequest<UserDetailsResponseDto>
    {
        public string Username { get; set; }
    }
}
