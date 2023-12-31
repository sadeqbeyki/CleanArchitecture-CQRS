using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetProductsByEmailQuery(string email) : IRequest<IEnumerable<ProductDetailsDto>>;
