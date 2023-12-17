using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.QueryHandlers
{
    public sealed class GetUserDetailsQueryHandler :
        IRequestHandler<GetUserDetailsQuery, UserDetailsResponseDto>
    {
        private readonly IIdentityService _identityService;

        public GetUserDetailsQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserDetailsResponseDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            var (userId, userName, firstName, lastName, email, phoneNumber, roles) =
                await _identityService.GetUserDetailsAsync(request.UserId);
            return new UserDetailsResponseDto()
            {
                Id = userId,
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                Roles = roles
            };
        }
    }
}
