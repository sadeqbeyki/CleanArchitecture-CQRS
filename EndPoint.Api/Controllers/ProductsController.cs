﻿using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Infrastructure.ACL;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager,Member")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserServiceACL _userServiceACL;
        public ProductsController(IMediator mediator, IUserServiceACL userServiceACL)
        {
            _mediator = mediator;
            _userServiceACL = userServiceACL;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _mediator.Send(new GetAllProductQuery());
            return Ok(result);
        }

        [HttpGet("GetProductsByEmail/{email}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByEmail(string userEmail)
        {
            var result = await _mediator.Send(new GetProductsByUserNameQuery(userEmail));
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand createCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createCommand, cancellationToken);
            return Ok(result);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody]UpdateProductCommand updateCommand,
            CancellationToken cancellationToken)
        {
            var user = await _userServiceACL.GetCurrentUserByClaimAsync(User);
            //if (user.Email != request.ManufacturerEmail)
            //{
            //    return BadRequest("You can only edit products that you have created yourself.");
            //}
            var result = await _mediator.Send(updateCommand, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);
            return Ok(result);
        }
    }
}
