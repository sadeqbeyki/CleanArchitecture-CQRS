using Identity.Application.DTOs;
using Identity.Application.DTOs.Auth;
using Identity.Persistance.Identity;

namespace Identity.Application.Interface
{
    public interface IIdentityService : IServiceBase
    {
        //Role
        Task<bool> CreateRoleAsync(string roleName);
        Task<List<(string id, string roleName)>> GetRolesAsync();
        Task<(string id, string roleName)> GetRoleByIdAsync(string id);
        Task<bool> UpdateRole(string id, string roleName);
        Task<bool> DeleteRoleAsync(string roleId);

        // User's Role
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> AssignUserToRole(string userName, IList<string> roles);
        Task<bool> UpdateUsersRole(string userName, IList<string> usersRole);

        //User
        Task<(bool isSucceed, string userId)> CreateUserAsync(RegisterUserDto userDto);
        Task<UserDetailsDto> GetUserDetailsAsync(string userId);
        Task<List<(string id, string userName, string firstName, string lastName, string email, string phoneNumber)>> GetAllUsersAsync();
        Task<bool> UpdateUser(string id, string firstName, string lastName, string email, IList<string> roles);
        Task<bool> DeleteUserAsync(string userId);

        //User more option
        Task<string> GetUserIdAsync(string userName);
        Task<string> GetUserNameAsync(string userId);
        Task<(string userId, string userName, string firstName, string lastName, string email, string phoneNumber, IList<string> roles)> GetUserDetailsByUserNameAsync(string userName);
        Task<bool> IsUniqueUserName(string userName);

        Task<UserDetailsDto> GetUserDetailsAsync(ApplicationUser user);
        Task<ApplicationUser> GetUserByNameAsync(string userName);
        Task<ApplicationUser> GetUserByIdAsync(string id);

        //Account
        Task<bool> SigninUserAsync(LoginUserDto dto);
        Task<JwtTokenDto> GetJwtSecurityTokenAsync(ApplicationUser user);
    }
}
