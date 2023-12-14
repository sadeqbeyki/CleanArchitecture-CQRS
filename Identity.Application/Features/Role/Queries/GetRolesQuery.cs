using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.Role.Queries;

public class GetRolesQuery : IRequest<IEnumerable<RoleResponseDto>>
{
}
