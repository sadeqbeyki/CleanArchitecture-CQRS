using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetAllProductQuery : IRequest<IEnumerable<Product>>;

