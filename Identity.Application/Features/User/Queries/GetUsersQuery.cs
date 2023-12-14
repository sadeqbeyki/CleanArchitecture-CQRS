using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.User.Queries
{
    public class GetUsersQuery : IRequest<List<UserResponseDto>>
    {
    }
}
