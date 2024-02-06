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
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            AuthCommand command = new(dto);
            var result = await _mediator.Send(command);

            if (!string.IsNullOrEmpty(result.Token))
            {
                Response.Cookies.Append("Token", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                });
                return RedirectToAction("Index", "Home");
            }

            return BadRequest("User is not authenticated");
        }


    }
}
