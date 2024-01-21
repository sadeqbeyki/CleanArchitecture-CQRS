using Application.DTOs;
using Application.DTOs.ProductCategories;
using Application.Exceptions;
using Application.Features.ProductCategories.Query;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using MediatR;

namespace Application.Features.ProductCategories.QueryHandler;
public class GetAllProductCategoryQueryHandler : IRequestHandler<GetAllProductCategoryQuery, IEnumerable<ProductCategoryDto>>
{
    private readonly IProductCategoryQueryService _productCategoryQueryService;

    public GetAllProductCategoryQueryHandler(IProductCategoryQueryService productCategoryQueryService)
    {
        _productCategoryQueryService = productCategoryQueryService;
    }

    public async Task<IEnumerable<ProductCategoryDto>> Handle(GetAllProductCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _productCategoryQueryService.GetProductCategories();
        if (result.Count == 0)
        {
            var exception = new NotFoundException($"No products found ");
            throw exception;
        }
        return result;
    }
}

