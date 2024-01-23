using Application.Interface;
using Identity.Application.DTOs.Auth;
using Identity.Application.Features.Auth.Command;
using Identity.Persistance.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;

namespace EndPoint.WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private new readonly UserManager<ApplicationUser> _userManager;


        public AuthController(IMediator mediatR, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediatR;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        //[ProducesDefaultResponseType(typeof(JwtTokenDto))]

        [HttpGet]
        public async Task<ActionResult<LoginUserDto>> Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesDefaultResponseType(typeof(JwtTokenDto))]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            AuthCommand command = new(dto);
            var result = await _mediator.Send(command);

            var identity = new GenericIdentity(result.User.UserName);
            var principal = new GenericPrincipal(identity, new string[0]);
            HttpContext.User = principal;
            Thread.CurrentPrincipal = principal;

            if (HttpContext.User.Identity!.IsAuthenticated)
                if (!string.IsNullOrEmpty(result.Token))
                {
                    Cookie cookie = new("JwtToken", result.Token);
                    cookie.Expires = result.ExpireOn;
                    Response.Cookies.Append("JwtToken", result.Token);

                    return RedirectToAction("Index", "Home", new { Token = result.Token });
                }
            return BadRequest("User is not authenticated");
        }
    }
}
