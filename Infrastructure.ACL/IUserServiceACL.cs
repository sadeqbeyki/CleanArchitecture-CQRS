using Identity.Application.Interface;
using Identity.Persistance.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.ACL;

public interface IUserServiceACL
{
    //Task<UserInfo> GetCurrentUserByEmailAsync(string email);

    Task<UserInfo> GetCurrentUserByClaimAsync(ClaimsPrincipal user);
    Task<UserInfo> GetCurrentUser();
}

public class UserInfo
{
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}

public class UserServiceACL : IUserServiceACL
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IIdentityService _identityService;

    public UserServiceACL(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IIdentityService identityService)
    {
        _userManager = userManager;
        _contextAccessor = contextAccessor;
        _identityService = identityService;
    }

    //public async Task<UserInfo> GetCurrentUserByEmailAsync(string email)
    //{
    //    var applicationUser = await _userManager.FindByEmailAsync(email);

    //    return new UserInfo
    //    {
    //        Email = applicationUser?.Email,
    //        PhoneNumber = applicationUser?.PhoneNumber,
    //    };
    //}

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
