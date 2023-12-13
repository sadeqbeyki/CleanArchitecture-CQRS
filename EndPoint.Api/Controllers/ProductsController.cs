using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _mediator.Send(new GetAllProductQuery());
            return Ok(result);
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct(CreateProductCommand createProduct,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(createProduct, cancellationToken);
            return Ok(result);
        }
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand updateProduct,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(updateProduct, cancellationToken);
            return Ok(result);
        }
    }
}
