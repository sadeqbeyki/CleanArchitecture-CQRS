using MediatR;

namespace Identity.Application.Features.User.Commands;
public class UpdateUserCommand : IRequest<int>
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}

//public record UpdateUserCommands(
//    string id,
//    string fullName,
//    string email,
//    List<string> roles) : IRequest<int>;

