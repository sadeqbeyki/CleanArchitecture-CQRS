using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.QueryHandlers
{
    public sealed class GetAllUsersDetailsQueryHandler :
        IRequestHandler<GetAllUsersDetailsQuery, List<UserDetailsDto>>
    {
        private readonly IIdentityService _identityService;

        public GetAllUsersDetailsQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<UserDetailsDto>> Handle(GetAllUsersDetailsQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();

            foreach (var user in users)
            {
                user.Roles = await _identityService.GetUserRolesAsync(user.UserId);
            }
            return users;
        }
    }
}
