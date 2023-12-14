using Identity.Application.Features.Role.Command;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.Role.CommandHandlers
{
    public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
    {
        private readonly IIdentityService _identityService;

        public CreateRoleCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateRoleAsync(request.RoleName);
            return result ? 1 : 0;
        }
    }
}
