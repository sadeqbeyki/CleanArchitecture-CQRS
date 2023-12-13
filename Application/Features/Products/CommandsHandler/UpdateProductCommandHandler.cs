﻿using Application.Features.Products.Commands;
using Application.Interface;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Products.CommandsHandler;

public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
{
    private readonly IProductDbContext _productDbContext;

    public UpdateProductCommandHandler(IProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<int> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(p => p.Id == request.Id);
        if (product != null)
        {
            product.Edit(
            request.Name,
            request.ManufacturePhone,
            request.ManufactureEmail);
            await _productDbContext.SaveChangeAsync();
            return product.Id;
        }
        return default;
    }
}
