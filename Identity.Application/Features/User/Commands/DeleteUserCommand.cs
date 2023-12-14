using MediatR;

namespace Identity.Application.Features.User.Commands
{
    public class DeleteUserCommand : IRequest<int>
    {
        public string Id { get; set; }
    }
}
