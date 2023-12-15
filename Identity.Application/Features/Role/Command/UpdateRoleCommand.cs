using MediatR;

namespace Identity.Application.Features.Role.Command
{
    public class UpdateRoleCommand : IRequest<int>
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
    }
}
