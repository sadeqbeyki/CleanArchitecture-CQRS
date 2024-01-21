using Application.DTOs;
using Application.DTOs.ProductCategories;
using Application.Exceptions;
using Application.Features.ProductCategories.Query;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using MediatR;

namespace Application.Features.ProductCategories.QueryHandler;
public class GetAllProductCategoryQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, IEnumerable<ProductCategoryDto>>
{
    private readonly IProductQueryService _productQueryService;

    public GetAllProductCategoryQueryHandler(IProductQueryService productQueryService)
    {
        _productQueryService = productQueryService;
    }

    public async Task<IEnumerable<ProductDetailsDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var result = await _productQueryService.GetProducts();
        if (result.Count == 0)
        {
            var exception = new NotFoundException($"No products found ");
            throw exception;
        }
        return result;
    }
}

