using Identity.Application.Features.User.Commands;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.CommandHandlers
{
    public sealed class UpdateUserCommandHandler:IRequestHandler<UpdateUserCommand, int>
    {
    private readonly IIdentityService _identityService;

        public UpdateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityService.UpdateUser(request.dto);
            return user ? 1 : 0;
        }
    }
}
