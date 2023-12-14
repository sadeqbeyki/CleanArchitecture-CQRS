using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs;
using Identity.Application.Features.Auth.Command;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.Auth.CommandHanlers
{
    internal sealed class AuthCommandHandler : IRequestHandler<AuthCommand, AuthResponseDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthCommandHandler(IIdentityService identityService,
            ITokenGenerator tokenGenerator)
        {
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthResponseDto> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.SigninUserAsync(request.UserName, request.Password);

            if (!result)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var (userId, fullName, userName, email, roles) = await _identityService.GetUserDetailsAsync(await _identityService.GetUserIdAsync(request.UserName));

            string token = _tokenGenerator.GenerateJWTToken((userId: userId, userName: userName, roles: roles));

            return new AuthResponseDto()
            {
                UserId = userId,
                Name = userName,
                Token = token
            };
        }
    }
}
