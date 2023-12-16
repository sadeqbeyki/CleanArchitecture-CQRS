using Domain.Products;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetProductsByUserNameQuery(string UserEmail) 
    : IRequest<IEnumerable<Product>>;
