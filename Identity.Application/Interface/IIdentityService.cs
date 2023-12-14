namespace Identity.Application.Interface
{
    public interface IIdentityService
    {
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string fullName, List<string> roles);

    }
}
