using Domain.Products;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetAllProductQuery : IRequest<IEnumerable<Product>>;
//public record GetAllProductQuery() : IRequest<IEnumerable<Product>>;

