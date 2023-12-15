using Identity.Application.DTOs.Auth;
using MediatR;

namespace Identity.Application.Features.Auth.Command
{
    public class AuthCommand : IRequest<JwtTokenDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
