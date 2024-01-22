using Application.DTOs;
using Application.DTOs.ProductCategories;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using AutoMapper;
using EndPoint.WebApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IProductCategoryQueryService _productCategoryQueryService;
        private readonly IProductQueryService _productQueryService;

        public ProductsController(IMediator mediator,
            IProductCategoryQueryService productCategoryQueryService,
            IProductQueryService productQueryService,
            IMapper mapper)
        {
            _mediator = mediator;
            _productCategoryQueryService = productCategoryQueryService;
            _productQueryService = productQueryService;
            _mapper = mapper;
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
        [HttpGet]
        public async Task<ActionResult<UpdateProductDto>> Update(Guid id)
        {
            var product = await _productQueryService.GetProductById(id);
            if (product == null)
                return RedirectToAction();
            UpdateProductViewModel model = new()
            {
                Product = product,
                ProductCategories = await _productCategoryQueryService.GetProductCategories(),
            };
            return View(model);

        }
        [HttpPut, HttpPost]
        public async Task<ActionResult> Update(UpdateProductViewModel model, CancellationToken cancellationToken)
        {
            try
            {
                if (model == null)
                    return View(model);

                var mapProduct = _mapper.Map<UpdateProductDto>(model.Product);
                var editCommand = new UpdateProductCommand(mapProduct);
                var result = await _mediator.Send(editCommand, cancellationToken);

                return RedirectToAction("Index", result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        #region Delete
        [HttpGet]
        public async Task<ActionResult<ProductDetailsDto>> Delete(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteProductCommand { Id = id }, cancellationToken);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
