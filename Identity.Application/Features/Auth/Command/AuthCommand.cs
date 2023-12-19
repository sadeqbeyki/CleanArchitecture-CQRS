using Identity.Application.DTOs.Auth;
using MediatR;

namespace Identity.Application.Features.Auth.Command;

public record AuthCommand(LoginUserDto dto) : IRequest<JwtTokenDto>;
