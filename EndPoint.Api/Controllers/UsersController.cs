using Application.Features.User.Commands;
using Identity.Application.DTOs;
using Identity.Application.Features.User.Commands;
using Identity.Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndPoint.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Manager" , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet("[action]")]
    [ProducesDefaultResponseType(typeof(UserDetailsDto))]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    [HttpGet("[action]/{id}")]
    [ProducesDefaultResponseType(typeof(UserDetailsResponseDto))]
    public async Task<IActionResult> GetAsync(string id)
    {
        var used = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetUserDetailsQuery(id));
        return Ok(result);
    }

    [HttpGet("[action]/{userName}")]
    [ProducesDefaultResponseType(typeof(UserDetailsDto))]
    public async Task<IActionResult> GetByUserName(string userName)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetUserByUserNameQuery(userName));
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    [ProducesDefaultResponseType(typeof(int))]
    public async Task<ActionResult> CreateAsync(CreateUserCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("[action]/{id}")]
    [ProducesDefaultResponseType(typeof(int))]
    public async Task<ActionResult> UpdateAsync(string id, [FromBody] UpdateUserCommand command)
    {
        if (id == command.dto.Id)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete("[action]/{userId}")]
    [ProducesDefaultResponseType(typeof(int))]
    public async Task<IActionResult> DeleteAsync(string userId)
    {
        var result = await _mediator.Send(new DeleteUserCommand() { Id = userId });
        return Ok(result);
    }

    [HttpPost("[action]")]
    [ProducesDefaultResponseType(typeof(int))]
    public async Task<ActionResult> AssignRoles(AssignUserToRoleCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("[action]")]
    [ProducesDefaultResponseType(typeof(int))]
    public async Task<ActionResult> UpdateUserRoles(UpdateUserRolesCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
