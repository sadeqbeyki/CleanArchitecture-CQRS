using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.Auth;
using Identity.Application.Features.Auth.Command;
using Identity.Application.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Identity.Application.Features.Auth.CommandHanlers;

internal sealed class AuthCommandHandler : IRequestHandler<AuthCommand, JwtTokenDto>
{
    private readonly IIdentityService _identityService;

    public AuthCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<JwtTokenDto> Handle(AuthCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.SigninUser(request.dto)
            ?? throw new BadRequestException("Invalid username or password");


        JwtTokenDto token = await _identityService.GetJwtSecurityTokenAsync(result);
        return token;
    }
}
