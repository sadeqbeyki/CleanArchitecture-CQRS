using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.Auth.Command
{
    public class AuthCommand : IRequest<AuthResponseDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
