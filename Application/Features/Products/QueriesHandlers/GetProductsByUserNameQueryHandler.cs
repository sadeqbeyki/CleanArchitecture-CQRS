using Application.DTOs;
using Application.Features.Products.Queries;
using Application.Interface.Query;
using Domain.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Features.Products.QueriesHandlers
{
    internal sealed class GetProductsByUserNameQueryHandler
        : IRequestHandler<GetProductsByUserNameQuery, IEnumerable<ProductDetailsDto>>
    {
        private readonly IProductQueryService _productQueryService;

        public GetProductsByUserNameQueryHandler(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        public async Task<IEnumerable<ProductDetailsDto>> Handle(GetProductsByUserNameQuery request, CancellationToken cancellationToken)
        {
            var result = _productQueryService.GetProductsByEmail(request.email);

            return result;
        }
    }
}
