using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.QueryHandlers
{
    public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserResponseDto>>
    {
        private readonly IIdentityService _identityService;

        public GetUsersQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<List<UserResponseDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _identityService.GetAllUsersAsync();
            var result = users.Select(u => new UserResponseDto()
            {
                Id = u.id,
                FullName = u.fullName,
                UserName = u.userName,
                Email = u.email,

            }).ToList();

            return result;
        }
    }
}
