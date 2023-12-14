using Identity.Application.DTOs;
using Identity.Application.Features.Role.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.Role.QuerieHandles;

public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleResponseDto>>
{
    private readonly IIdentityService _identityService;

    public GetRolesQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<IEnumerable<RoleResponseDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _identityService.GetRolesAsync();
        return roles.Select(role => new RoleResponseDto()
        {
            Id = role.id,
            RoleName = role.roleName
        }).ToList();
    }
}

