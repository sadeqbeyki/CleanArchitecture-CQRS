using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.Role.Queries
{
    public class GetRoleByIdQuery : IRequest<RoleResponseDto>
    {
        public string RoleId { get; set; }
    }
}
