using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.QueryHandlers
{
    public sealed class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsResponseDto>
    {
        private readonly IIdentityService _identityService;

        public GetUserDetailsQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserDetailsResponseDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var (userId, fullName, userName, email, roles) =
                await _identityService.GetUserDetailsRolesAsync(request.UserId);
            return new UserDetailsResponseDto()
            {
                Id = userId,
                FullName = fullName,
                UserName = userName,
                Email = email,
                Roles = roles
            };
        }
    }
}
