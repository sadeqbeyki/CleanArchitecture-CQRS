using Identity.Application.Features.User.Commands;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.CommandHandlers
{
    internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
    {
        private readonly IIdentityService _identityService;

        public DeleteUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.DeleteUserAsync(request.Id);
            return result ? 1 : 0;
        }
    }
}
