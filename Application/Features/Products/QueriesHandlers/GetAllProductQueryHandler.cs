using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using MediatR;

namespace Application.Features.Products.QueriesHandlers;
public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<ProductDetailsDto>>
{
    private readonly IProductQueryService _productQueryService;

    public GetAllProductQueryHandler(IProductQueryService productQueryService)
    {
        _productQueryService = productQueryService;
    }

    public async Task<IEnumerable<ProductDetailsDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var result = _productQueryService.GetProducts();
        return result;
    }
}

