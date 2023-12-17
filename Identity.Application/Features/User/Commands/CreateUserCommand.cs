using Identity.Application.DTOs.Auth;
using MediatR;

namespace Application.Features.User.Commands;

public record CreateUserCommand(RegisterUserDto dto) : IRequest<int>;

