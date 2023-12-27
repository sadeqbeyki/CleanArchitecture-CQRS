using Application.Features.User.Commands;
using Identity.Application.DTOs;
using Identity.Application.Features.User.Commands;
using Identity.Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin, Manager")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(int))]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUser(CreateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("GetAllUserDetails")]
        [ProducesDefaultResponseType(typeof(UserDetailsDto))]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }

        [HttpGet("GetUserDetails/{userId}")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDto))]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var result = await _mediator.Send(new GetUserDetailsQuery(userId));
            return Ok(result);
        }

        [HttpGet("GetUserDetailsByUserName/{userName}")]
        [ProducesDefaultResponseType(typeof(UserDetailsDto))]
        public async Task<IActionResult> GetUserByUserName(string userName)
        {
            var result = await _mediator.Send(new GetUserByUserNameQuery(userName));
            return Ok(result);
        }

        [HttpPut("UpdateUser/{id}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> UpdateUserAsync(string id, [FromBody] UpdateUserCommand command)
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

        [HttpDelete("Delete/{userId}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand() { Id = userId });
            return Ok(result);
        }

        [HttpPost("AssignRoles")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> AssignRoles(AssignUserToRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateUserRoles")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> UpdateUserRoles(UpdateUserRolesCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
