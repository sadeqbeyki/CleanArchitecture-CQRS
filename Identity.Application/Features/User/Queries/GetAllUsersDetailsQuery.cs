using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.User.Queries;

public record GetAllUsersDetailsQuery: IRequest<List<UserDetailsDto>>;

