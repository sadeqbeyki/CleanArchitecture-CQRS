using MediatR;

namespace Application.Features.Products.Commands;
public class DeleteProductCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
};


