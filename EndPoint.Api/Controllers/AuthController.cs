﻿using Identity.Application.Features.Auth.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.DTOs.Auth;
using Identity.Application.Features.Auth.Query;

namespace EndPoint.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("Login")]
    [ProducesDefaultResponseType(typeof(JwtTokenDto))]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        var result = await _mediator.Send(new AuthCommand(dto));

        if (User.Identity.IsAuthenticated)
            return Ok(result);
        return Unauthorized("failed authentication!");
    }

    [HttpGet]
    [ProducesDefaultResponseType(typeof(JwtTokenDto))]
    public async Task<IActionResult> RefreshToken()
    {
        var result = await _mediator.Send(new GetTokenQuery());
        if (result == null)
            return Unauthorized();
        return Ok(result);
    }
}
