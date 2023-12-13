using MediatR;

namespace Application.Features.Products.Commands;
public class DeleteProductCommand : IRequest<int>
{
    public int Id { get; set; }
};


