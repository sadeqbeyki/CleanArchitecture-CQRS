using Application.DTOs;
using Application.Features.Products.Commands;
using Application.Interface.Command;
using MediatR;

namespace Application.Features.Products.CommandsHandler;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDetailsDto>
{
    private readonly IProductCommandService _commandService;

    public CreateProductCommandHandler(IProductCommandService commandService)
    {
        _commandService = commandService;
    }

    public async Task<ProductDetailsDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await _commandService.AddProduct(request.dto);
        return result;
    }
}
