using Application.DTOs;
using Application.DTOs.ProductCategories;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using EndPoint.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IProductCategoryQueryService _productCategoryQueryService;

        public ProductsController(IMediator mediator, IProductCategoryQueryService productCategoryQueryService)
        {
            _mediator = mediator;
            _productCategoryQueryService = productCategoryQueryService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProductDetailsDto>>> Index()
        {
            var products = await _mediator.Send(new GetAllProductQuery());
            return View(products);
        }
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> Details(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return View(product);
        }

        public async Task<ActionResult<AddProductDto>> Create()
        {
            var model = new CreateProductViewModel()
            {
                ProductCategories = await _productCategoryQueryService.GetProductCategories()
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Create(AddProductDto productDto, CancellationToken cancellationToken)
        {
            try
            {
                var createCommand = new CreateProductCommand(productDto);
                var result = await _mediator.Send(createCommand, cancellationToken);
                if (result.Id != Guid.Empty)
                    return RedirectToAction("Index");
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult<Guid>> Update([FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

    }
}
