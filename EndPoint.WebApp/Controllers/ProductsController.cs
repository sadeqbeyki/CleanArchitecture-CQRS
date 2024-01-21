using Application.DTOs;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using EndPoint.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult<List<ProductDetailsDto>>> Index()
        {
            var products = await _mediator.Send(new GetAllProductQuery());
            return View(products);
        }
        public async Task<ActionResult<ProductDetailsDto>> Details(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return View(product);
        }
        public async Task<ActionResult<ProductDetailsDto>> Create(CreateProductCommand command, CancellationToken cancellationToken)
        {
            CreateProductViewModel viewModel = new()
            {
                ProductCategories = _
            };
            var result = await _mediator.Send(command, cancellationToken);
            if (result.Id != Guid.Empty)
                return View(result);
            return View(command);
        }
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

    }
}
