﻿using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using Domain.Entities.Products;
using FluentValidation;
using Infrastructure.ACL;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EndPoint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager,Member")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserServiceACL _userServiceACL;
        private readonly IProductQueryService _productQueryService;
        public ProductsController(
            IMediator mediator,
            IUserServiceACL userServiceACL,
            IProductQueryService productQueryService
,
            ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _userServiceACL = userServiceACL;
            _productQueryService = productQueryService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _mediator.Send(new GetAllProductQuery());
            return Ok(result);
        }

        [HttpGet("GetProductsByEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByEmail([FromQuery] string email)
        {
            var result = await _mediator.Send(new GetProductsByEmailQuery(email));
            return Ok(result);
        }

        [HttpGet("GetProductsByEmailPhone")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByEmailPhone([FromQuery] string mailORphone)
        {
            var result = await _mediator.Send(new GetProductsByEmailPhoneQuery(mailORphone));
            return Ok(result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand createCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createCommand, cancellationToken);
            _logger.LogInformation("This log message will be sent to EventSource.");
            return Ok(result);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand updateCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateCommand, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);
            return Ok(result);
        }
    }
}
