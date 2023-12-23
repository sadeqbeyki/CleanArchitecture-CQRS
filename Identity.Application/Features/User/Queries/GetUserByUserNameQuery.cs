using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.User.Queries;

public record GetUserByUserNameQuery(string Username) : IRequest<UserDetailsDto>;

