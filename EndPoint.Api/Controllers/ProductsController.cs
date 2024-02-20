using Application.DTOs;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace EndPoint.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    public async Task<IActionResult> Get()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

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


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        if (result == null)
            return NotFound("No product with this ID was found!");
        return Ok(result);
    }


    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        var result = await _mediator.Send(new GetProductsByEmailQuery(email));
        return Ok(result);
    }

    [HttpGet("[action]")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByPhoneEmail([FromQuery] string phoneOrEmail)
    {
        var result = await _mediator.Send(new GetProductsByEmailPhoneQuery(phoneOrEmail));
        return StatusCode(StatusCodes.Status200OK);
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm]AddProductDto model, CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
         
        var result = await _mediator.Send(new CreateProductCommand(model), cancellationToken);
        if (result.Id != Guid.Empty)
            return StatusCode(StatusCodes.Status201Created);
        return StatusCode(StatusCodes.Status400BadRequest);
    }

    [HttpPut("[action]/{id}")]
    public async Task<IActionResult> Update([FromForm] UpdateProductCommand updateCommand,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(updateCommand, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("[action]/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);
        return Ok(result);
    }
}
