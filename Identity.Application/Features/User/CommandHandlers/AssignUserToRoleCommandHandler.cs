using Identity.Application.Features.User.Commands;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.CommandHandlers
{
    public sealed class AssignUserToRoleCommandHandler : IRequestHandler<AssignUserToRoleCommand, int>
    {
        private readonly IIdentityService _identityService;

        public AssignUserToRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.AssignUserToRole(request.Username, request.Roles);
            return result ? 1 : 0;
        }
    }
}
