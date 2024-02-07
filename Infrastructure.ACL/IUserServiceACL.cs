using Identity.Application.Interface;
using Identity.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.ACL;

public interface IUserServiceACL
{
    Task<UserInfo> GetCurrentUserByClaimAsync(ClaimsPrincipal user);
    Task<UserInfo> GetCurrentUser();
}

public class UserInfo
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}

public class UserServiceACL(
    UserManager<ApplicationUser> userManager,
    IIdentityService identityService,
    IHttpContextAccessor contextAccessor) : IUserServiceACL
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IIdentityService _identityService = identityService;
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    public async Task<UserInfo> GetCurrentUserByClaimAsync(ClaimsPrincipal user)
    {
        var applicationUser = await _userManager.GetUserAsync(user);

        return new UserInfo
        {
            Email = applicationUser?.Email,
            PhoneNumber = applicationUser?.PhoneNumber?.ToString(),
        };
    }

    public async Task<UserInfo> GetCurrentUser()
    {
        string userId = GetCurrentOperatorId();
        var user = await _identityService.GetUserByIdAsync(userId);
        var userDetails = new UserInfo
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        return userDetails;
    }
    private string GetCurrentOperatorId()
    {
        return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
