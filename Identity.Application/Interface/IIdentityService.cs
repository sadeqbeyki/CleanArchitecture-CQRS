namespace Identity.Application.Interface
{
    public interface IIdentityService
    {
        //role
        Task<bool> CreateRoleAsync(string roleName);
        Task<List<(string id, string roleName)>> GetRolesAsync();
        Task<(string id, string roleName)> GetRoleByIdAsync(string id);
        Task<bool> UpdateRole(string id, string roleName);
        Task<bool> DeleteRoleAsync(string roleId);

        //user
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string fullName, List<string> roles);
        Task<string> GetUserIdAsync(string userName);
        Task<List<(string id, string fullName, string userName, string email)>> GetAllUsersAsync();
        Task<bool> UpdateUserProfile(string id, string fullName, string email, IList<string> roles);
        Task<bool> DeleteUserAsync(string userId);

        //user more option
        Task<string> GetUserNameAsync(string userId);
        Task<(string userId, string fullName, string UserName, string email, IList<string> roles)> GetUserDetailsAsync(string userId);
        Task<(string userId, string fullName, string UserName, string email, IList<string> roles)> GetUserDetailsByUserNameAsync(string userName);
        Task<bool> IsUniqueUserName(string userName);

        //Account
        Task<bool> SigninUserAsync(string userName, string password);
    }
}
