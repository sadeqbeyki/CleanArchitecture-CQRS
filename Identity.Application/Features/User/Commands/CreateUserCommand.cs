using MediatR;

namespace Application.Features.User.Commands;

public class CreateUserCommand : IRequest<int>
{
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmationPassword { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
