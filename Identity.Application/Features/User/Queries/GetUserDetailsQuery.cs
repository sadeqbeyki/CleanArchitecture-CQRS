﻿using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Features.User.Queries;

public record GetUserDetailsQuery(string UserId) : IRequest<UserDetailsDto>;
