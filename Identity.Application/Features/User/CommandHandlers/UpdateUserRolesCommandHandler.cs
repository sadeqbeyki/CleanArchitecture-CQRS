using Identity.Application.Features.User.Commands;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.CommandHandlers
{
    public sealed class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, int>
    {
        private readonly IIdentityService _identityService;

        public UpdateUserRolesCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateUsersRole(request.Username, request.Roles);
            return result ? 1 : 0;
        }
    }
}
