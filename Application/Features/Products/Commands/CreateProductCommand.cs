using Domain.Products;
using MediatR;

namespace Application.Features.Products.Commands;
public class CreateProductCommand : IRequest<int>
{
    public string Name { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
};

//public record CreateProductCommand(Product product) : IRequest;

