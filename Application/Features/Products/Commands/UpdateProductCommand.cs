using Application.DTOs;
using MediatR;

namespace Application.Features.Products.Commands;
public record UpdateProductCommand(UpdateProductDto Dto) : IRequest<Guid>;


