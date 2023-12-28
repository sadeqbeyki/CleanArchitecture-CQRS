using Application.DTOs;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetProductsByUserNameQuery(string email) : IRequest<IEnumerable<ProductDetailsDto>>;
