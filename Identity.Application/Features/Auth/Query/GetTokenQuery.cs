using Identity.Application.DTOs.Auth;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.Auth.Query;

public record GetTokenQuery : IRequest<string>;

internal sealed class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, string>
{
    private readonly IIdentityService _identityService;

    public GetTokenQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var currentUser = await _identityService.GetCurrentUser();

        JwtTokenDto refreshedToken = await _identityService.GetJwtSecurityTokenAsync(currentUser);

        return refreshedToken.Token;
    }
}
