using MediatR;

namespace Identity.Application.Features.User.Commands
{
    public class AssignUserToRoleCommand : IRequest<int>
    {
        public string Username { get; set; }
        public IList<string> Roles { get; set; }
    }
}
