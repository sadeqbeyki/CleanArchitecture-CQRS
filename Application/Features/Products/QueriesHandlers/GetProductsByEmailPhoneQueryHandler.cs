using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using MediatR;

namespace Application.Features.Products.QueriesHandlers;

internal sealed class GetProductsByEmailPhoneQueryHandler
    : IRequestHandler<GetProductsByEmailPhoneQuery, List<ProductDetailsDto>>
{
    private readonly IProductQueryService _productQueryService;

    public GetProductsByEmailPhoneQueryHandler(IProductQueryService productQueryService)
    {
        _productQueryService = productQueryService;
    }

    public async Task<List<ProductDetailsDto>> Handle(GetProductsByEmailPhoneQuery request, CancellationToken cancellationToken)
    {
        var result = await _productQueryService.GetProductsByEmailPhone(request.name);

        return result;
    }
}
