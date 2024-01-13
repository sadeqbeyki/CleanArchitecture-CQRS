using Application.DTOs;
using Application.Exceptions;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Products.QueriesHandlers
{
    internal sealed class GetProductsByEmailQueryHandler
        : IRequestHandler<GetProductsByEmailQuery, IEnumerable<ProductDetailsDto>>
    {
        private readonly IProductQueryService _productQueryService;
        private readonly ILogger<GetProductsByEmailQueryHandler> _logger;

        public GetProductsByEmailQueryHandler(IProductQueryService productQueryService, ILogger<GetProductsByEmailQueryHandler> logger)
        {
            _productQueryService = productQueryService;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDetailsDto>> Handle(GetProductsByEmailQuery request, CancellationToken cancellationToken)
        {
            var result = await _productQueryService.GetProductsByEmail(request.email);
            if (result.Count == 0)
            {
                var exception = new NotFoundException($"No products were found with this email {request.email} !");
                throw exception;
            }
            return result;
        }
    }
}
