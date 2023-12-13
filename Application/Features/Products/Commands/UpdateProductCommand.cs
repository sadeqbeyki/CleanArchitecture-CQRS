using MediatR;

namespace Application.Features.Products.Commands;
public class UpdateProductCommand : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }
};


