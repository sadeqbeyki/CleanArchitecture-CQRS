using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.QueryHandlers
{
    public sealed class GetUserByUserNameQueryHandler :
        IRequestHandler<GetUserByUserNameQuery, UserDetailsDto>
    {
        private readonly IIdentityService _identityService;

        public GetUserByUserNameQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserDetailsDto> Handle(GetUserByUserNameQuery request,
            CancellationToken cancellationToken)
        {
            UserDetailsDto model = await _identityService.GetUserByUserNameAsync(request.Username);
            return model;
        }
    }
}
