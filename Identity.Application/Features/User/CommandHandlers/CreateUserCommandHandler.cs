﻿using Application.Features.User.Commands;
using Identity.Application.Interface;
using MediatR;

namespace Identity.Application.Features.User.CommandHandlers;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IIdentityService _identityService;
    public CreateUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _identityService.CreateUserAsync(request.dto);
        return result.isSucceed ? 1 : 0;
    }
}


