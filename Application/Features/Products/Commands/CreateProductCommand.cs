using Domain.Products;
using MediatR;

namespace Application.Features.Products.Commands;
public class CreateProductCommand : IRequest<int>
{
    public string Name { get; set; }
    public string ManufacturerPhone { get; set; }
    public string ManufacturerEmail { get; set; }
};
