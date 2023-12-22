using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.QueryHandlers
{
    public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDetailsDto>>
    {
        private readonly IIdentityService _identityService;

        public GetUsersQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<UserDetailsDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();
            var result = users.Select(u => new UserDetailsDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,

            }).ToList();
            return result;
        }
    }
}
