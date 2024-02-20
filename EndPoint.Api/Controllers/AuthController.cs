using Identity.Application.Features.Auth.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.DTOs.Auth;
using Identity.Application.Interface;

namespace EndPoint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public AuthController(IMediator mediator,
            IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }


        [HttpPost("Login")]
        [ProducesDefaultResponseType(typeof(JwtTokenDto))]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var result = await _mediator.Send(new AuthCommand(dto));

            if (User.Identity.IsAuthenticated)
                return Ok(result.Token);
            return BadRequest("faild");
        }

        /// <summary>
        /// Create a new token for logged in user
        /// </summary>
        /// <returns>New token details</returns>
        [HttpGet]
        [ProducesDefaultResponseType(typeof(JwtTokenDto))]
        public async Task<IActionResult> RefreshToken()
        {
            // Obtain logged in user
            var currentUser = await _identityService.GetCurrentUser();
            if (currentUser == null)
                return Unauthorized();

            // Generate new token
            JwtTokenDto refreshedToken = await _identityService.GetJwtSecurityTokenAsync(currentUser);
            if (refreshedToken == null)
                return Unauthorized();

            return Ok(refreshedToken);
        }
    }
}
