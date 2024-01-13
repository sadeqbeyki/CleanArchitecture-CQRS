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
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IProductQueryService productQueryService,
        ILogger<GetProductByIdQueryHandler> logger)
    {
        _productQueryService = productQueryService;
        _logger = logger;
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

