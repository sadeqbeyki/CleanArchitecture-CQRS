using Identity.Application.DTOs.Auth;
using MediatR;

namespace Identity.Application.Features.User.Commands;
public record UpdateUserCommand(UpdateUserDto dto) : IRequest<int>;
