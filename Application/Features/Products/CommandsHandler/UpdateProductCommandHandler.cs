using Application.DTOs;
using Application.Features.Products.Commands;
using Application.Interface.Command;
using MediatR;

namespace Application.Features.Products.CommandsHandler;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
{
    private readonly IProductCommandService _commandService;

    public UpdateProductCommandHandler(IProductCommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _commandService.UpdateProduct(request.Dto);

        return product;
    }
}
