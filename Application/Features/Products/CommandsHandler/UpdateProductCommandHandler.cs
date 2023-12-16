using Application.Exceptions;
using Application.Features.Products.Commands;
using Domain.Entities.Products;
using Infrastructure.ACL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Features.Products.CommandsHandler;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
{
    private readonly IProductDbContext _productDbContext;
    private readonly IUserServiceACL _userServiceACL;

    public UpdateProductCommandHandler(
        IProductDbContext productDbContext, IUserServiceACL userServiceACL)
    {
        _productDbContext = productDbContext;
        _userServiceACL = userServiceACL;
    }

    public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

        if (product != null)
        {
            product.Edit(
            request.Name,
            request.ManufacturerPhone,
            request.ManufacturerEmail);
            await _productDbContext.SaveChangeAsync();
            return product.Id;
        }
        throw new NotFoundException(nameof(Product), request.Id);
    }
}
