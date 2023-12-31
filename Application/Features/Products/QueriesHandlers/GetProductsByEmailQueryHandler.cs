using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using MediatR;

namespace Application.Features.Products.QueriesHandlers
{
    internal sealed class GetProductsByEmailQueryHandler
        : IRequestHandler<GetProductsByEmailQuery, IEnumerable<ProductDetailsDto>>
    {
        private readonly IProductQueryService _productQueryService;

        public GetProductsByEmailQueryHandler(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        public async Task<IEnumerable<ProductDetailsDto>> Handle(GetProductsByEmailQuery request, CancellationToken cancellationToken)
        {
            var result = _productQueryService.GetProductsByEmail(request.email);

            return result;
        }
    }
}
