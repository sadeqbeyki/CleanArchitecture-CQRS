using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Commands;
public record CreateProductCommand(AddProductDto dto) : IRequest<ProductDetailsDto>;
