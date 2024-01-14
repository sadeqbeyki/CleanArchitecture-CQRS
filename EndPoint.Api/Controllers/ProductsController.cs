﻿using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace EndPoint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager,Member")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
             IMediator mediator,
             ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var result = await _mediator.Send(new GetAllProductQuery());
                _logger.LogInformation("Get Products Method Called!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message, ex);
                return BadRequest("Cant find any product!");
            }
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(CreateProductCommand createCommand,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createCommand, cancellationToken);
            if (result.Id != Guid.Empty)
                return StatusCode(StatusCodes.Status201Created);
            return StatusCode(StatusCodes.Status400BadRequest);
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
