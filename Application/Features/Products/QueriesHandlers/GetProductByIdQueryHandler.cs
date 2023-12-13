﻿using Application.Features.Products.Queries;
using Application.Interface;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            throw new Exception("Product not found!");
        }
        return result;
    }

}
