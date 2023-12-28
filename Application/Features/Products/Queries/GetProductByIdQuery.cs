using Application.DTOs;
using Domain.Entities.Products;
using MediatR;

namespace Application.Features.Products.Queries;

public class GetProductByIdQuery : IRequest<ProductDetailsDto>
{
    public Guid Id { get; set; }
}