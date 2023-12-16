using Application.Exceptions;
using Application.Features.Products.Commands;
using Domain.Entities.Products;
using Infrastructure.ACL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Features.Products.CommandsHandler;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, int>
{
    private readonly IProductDbContext _productDbContext;
    private readonly IUserServiceACL _userServiceACL;

    public DeleteProductCommandHandler(IProductDbContext productDbContext,
        IUserServiceACL userServiceACL)
    {
        _productDbContext = productDbContext;
        _userServiceACL = userServiceACL;
    }

    public async Task<int> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id)
            ?? throw new NotFoundException(nameof(Product), request.Id);

        var user = await _userServiceACL.GetCurrentUserByEmailAsync(product.ManufacturerEmail)
            ?? throw new NotFoundException(request.Id);

        if (product.ManufacturerEmail != user.Email)
        {
            throw new BadRequestException("You can only delete products that you have created yourself");
        }
        _productDbContext.Products.Remove(product);
        await _productDbContext.SaveChangeAsync();
        return request.Id;
    }
}
