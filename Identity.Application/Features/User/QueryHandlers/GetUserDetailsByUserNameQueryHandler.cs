using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.QueryHandlers
{
    public sealed class GetUserDetailsByUserNameQueryHandler :
        IRequestHandler<GetUserDetailsByUserNameQuery, UserDetailsResponseDto>
    {
        private readonly IIdentityService _identityService;

        public GetUserDetailsByUserNameQueryHandler(
            IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserDetailsResponseDto> Handle(
            GetUserDetailsByUserNameQuery request,
            CancellationToken cancellationToken)
        {
            var (userId, userName, firstName, lastName, email, phoneNumber, roles)
                = await _identityService.GetUserDetailsByUserNameAsync(request.Username);
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
