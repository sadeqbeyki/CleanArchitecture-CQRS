using Application.Features.Products.Commands;
using Application.Interface;
using Domain.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.CommandsHandler
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductDbContext _productDbContext;

        public CreateProductCommandHandler(IProductDbContext productDbContext)
        {
            _productDbContext = productDbContext;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(
                request.Name,
                request.ManufacturePhone,
                request.ManufactureEmail);

            await _productDbContext.Products.AddAsync(product);
            await _productDbContext.SaveChangeAsync();

            return product.Id;
        }
    }
}
