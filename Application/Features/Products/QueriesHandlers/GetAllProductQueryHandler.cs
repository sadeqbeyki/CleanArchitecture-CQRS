using Application.Features.Products.Queries;
using Domain.Entities.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Features.Products.QueriesHandlers;
public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
{
    private readonly IProductDbContext _productDbContext;

    public GetAllProductQueryHandler(IProductDbContext productDbContext)
        => _productDbContext = productDbContext;

    public async Task<IEnumerable<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var result = await _productDbContext.Products.ToListAsync(cancellationToken);
        return result;
    }
}

