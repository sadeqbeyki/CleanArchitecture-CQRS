using Application.DTOs;
using Application.Exceptions;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using Domain.Entities.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Products.QueriesHandlers;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDetailsDto>
{
    private readonly IProductQueryService _productQueryService;

    public GetProductByIdQueryHandler(IProductQueryService productQueryService)
    {
        _productQueryService = productQueryService;
    }

    public async Task<ProductDetailsDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _productQueryService.GetProductById(request.Id);
        if(result == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }
        return result;
    }
}

