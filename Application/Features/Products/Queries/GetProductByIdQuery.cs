using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<Product>
{
    public Guid Id { get; set; }
}