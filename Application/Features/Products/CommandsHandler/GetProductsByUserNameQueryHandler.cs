using Application.Features.Products.Queries;
using Domain.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Features.Products.CommandsHandler
{
    internal sealed class GetProductsByUserNameQueryHandler 
        : IRequestHandler<GetProductsByUserNameQuery, IEnumerable<Product>>
    {
        private readonly IProductDbContext _productDbContext;

        public GetProductsByUserNameQueryHandler(IProductDbContext productDbContext) 
            => _productDbContext = productDbContext;

        public async Task<IEnumerable<Product>> Handle(GetProductsByUserNameQuery request, CancellationToken cancellationToken)
        {
            var userEmail = request.UserEmail;

            var result = await _productDbContext.Products
                .Where(p => p.ManufacturerEmail == userEmail)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
