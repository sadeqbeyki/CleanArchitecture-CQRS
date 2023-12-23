using Application.Features.Products.Commands;
using Domain.Entities.Products;
using Identity.Application.Interface;
using Infrastructure.ACL;
using MediatR;
using Persistance;

namespace Application.Features.Products.CommandsHandler;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductDbContext _productDbContext;
    private readonly IUserServiceACL _userServiceACL;

    public CreateProductCommandHandler(
        IProductDbContext productDbContext,
        IUserServiceACL userServiceACL)
    {
        _productDbContext = productDbContext;
        _userServiceACL = userServiceACL;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var user = await _userServiceACL.GetCurrentUser();
        var product = new Product(
            request.Name,
            request.ManufacturerPhone,
            user.Email);

        await _productDbContext.Products.AddAsync(product);
        await _productDbContext.SaveChangeAsync();

        return product.Id;
    }
}
