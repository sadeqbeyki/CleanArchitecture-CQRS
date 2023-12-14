using Application.Exceptions;
using Application.Features.Products.Commands;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Features.Products.CommandsHandler;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
{
    private readonly IProductDbContext _productDbContext;

    public DeleteProductCommandHandler(IProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }
            _productDbContext.Products.Remove(product);
            await _productDbContext.SaveChangeAsync();
            return request.Id;
    }
}
