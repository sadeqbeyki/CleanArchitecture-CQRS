using Application.DTOs;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetAllProductQuery : IRequest<IEnumerable<ProductDetailsDto>>;

