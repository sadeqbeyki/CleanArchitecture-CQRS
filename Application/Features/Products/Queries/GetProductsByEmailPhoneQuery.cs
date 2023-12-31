using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Queries;

public record GetProductsByEmailPhoneQuery(string name) : IRequest<List<ProductDetailsDto>>;
