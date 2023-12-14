using Identity.Application.Features.Role.Command;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.Role.CommandHandlers;

internal sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, int>
{
    private readonly IIdentityService _identityService;

    public DeleteRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<int> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.DeleteRoleAsync(request.RoleId);
        return result ? 1 : 0;
    }
}
