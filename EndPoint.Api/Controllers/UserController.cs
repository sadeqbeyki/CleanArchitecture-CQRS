using Application.Features.User.Commands;
using Identity.Application.DTOs;
using Identity.Application.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> CreateUser(CreateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(List<UserResponseDto>))]
        public async Task<IActionResult> GetAllUserAsync()
        {
            return Ok(await _mediator.Send(new GetUsersQuery()));
        }

        [HttpGet("GetUserDetails/{userId}")]
        [ProducesDefaultResponseType(typeof(UserDetailsResponseDto))]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var result = await _mediator.Send(new GetUserDetailsQuery() { UserId = userId });
            return Ok(result);
        }


    }
}
