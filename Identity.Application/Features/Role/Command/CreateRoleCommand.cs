using MediatR;

namespace Identity.Application.Features.Role.Command;

public class CreateRoleCommand : IRequest<int>
{
    public string RoleName { get; set; }
}
