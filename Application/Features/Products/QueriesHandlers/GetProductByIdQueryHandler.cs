using Application.Exceptions;
using Application.Features.Products.Queries;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Features.Products.QueriesHandlers;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly IProductDbContext _productDbContext;

    public GetProductByIdQueryHandler(IProductDbContext productDbContext)
        => _productDbContext = productDbContext;

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _productDbContext.Products
            .Where(p => p.Id == request.Id).FirstOrDefaultAsync();
        if (result == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }
        return result;
    }

}

