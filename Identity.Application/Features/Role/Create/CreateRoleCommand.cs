using MediatR;

namespace Identity.Application.Features.Role.Create;

public class CreateRoleCommand : IRequest<int>
{
    public string RoleName { get; set; }
}
