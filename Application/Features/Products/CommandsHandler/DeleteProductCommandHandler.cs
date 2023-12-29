using Application.Exceptions;
using Application.Features.Products.Commands;
using Application.Interface.Command;
using Domain.Entities.Products;
using Infrastructure.ACL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;
using System.Runtime.CompilerServices;

namespace Application.Features.Products.CommandsHandler;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Guid>
{
    private readonly IProductDbContext _productDbContext;
    private readonly IUserServiceACL _userServiceACL;
    private readonly IProductCommandService _commandService;

    public DeleteProductCommandHandler(IProductDbContext productDbContext,
        IUserServiceACL userServiceACL,
        IProductCommandService commandService)
    {
        _productDbContext = productDbContext;
        _userServiceACL = userServiceACL;
        _commandService = commandService;
    }

    public async Task<Guid> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _commandService.DeleteProduct(request.Id);
        return result;
    }
}
