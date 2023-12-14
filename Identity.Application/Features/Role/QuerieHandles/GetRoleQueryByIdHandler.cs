using Identity.Application.DTOs;
using Identity.Application.Features.Role.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.Role.QuerieHandles
{
    public sealed class GetRoleQueryByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleResponseDto>
    {
        private readonly IIdentityService _identityService;

        public GetRoleQueryByIdHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<RoleResponseDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _identityService.GetRoleByIdAsync(request.RoleId);
            return new RoleResponseDto() { Id = role.id, RoleName = role.roleName };
        }
    }
}
