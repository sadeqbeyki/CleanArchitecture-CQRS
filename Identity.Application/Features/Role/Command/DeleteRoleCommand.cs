using MediatR;

namespace Identity.Application.Features.Role.Command;

public class DeleteRoleCommand: IRequest<int>
{
    public string RoleId { get; set; }
}
