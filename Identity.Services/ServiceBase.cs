using AutoMapper;
using Identity.Application.Common.Const;
using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs.AppSettings;
using Identity.Application.Interface;
using Identity.Persistance.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Identity.Services;

/// <summary>
///   Base class for Business Logic
/// </summary>
public class ServiceBase<TService> : IServiceBase where TService : class
{
    #region Fields & Ctor

    protected readonly ILogger<TService> _logger;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly IMapper _mapper;

    protected readonly AppSettings _appSettings;
    protected readonly UserManager<ApplicationUser> _userManager;

    protected readonly IStringLocalizerFactory _localizerFactory;
    protected readonly IStringLocalizer _sharedLocalizer;

    protected ServiceBase(IServiceProvider serviceProvider)
    {
        _logger = (ILogger<TService>)serviceProvider.GetService(typeof(ILogger<TService>));
        _httpContextAccessor = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
        _mapper = (IMapper)serviceProvider.GetService(typeof(IMapper));

        _appSettings = (AppSettings)serviceProvider.GetService(typeof(AppSettings));
        _userManager = (UserManager<ApplicationUser>)serviceProvider.GetService(typeof(UserManager<ApplicationUser>));

        _localizerFactory = (IStringLocalizerFactory)serviceProvider.GetService(typeof(IStringLocalizerFactory));
    }

    #endregion

    #region Current User related

    /// <summary>
    /// Obtain UserId from token
    /// </summary>
    /// <returns>UserId</returns>
    public string GetCurrentUserId()
    {
        ClaimsIdentity identity = _httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
        string userId = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userId ?? string.Empty;
    }

    /// <summary>
    /// Obtain AccountUser instance from JWT token that we get
    /// </summary>
    /// <returns>AccountUser for logged in user</returns>
    public async Task<ApplicationUser> GetCurrentUser()
    {
        // Obtain MailId from token
        ClaimsIdentity identity = _httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
        var userMailId = identity?.FindFirst(AppConstants.JWT_SUB)?.Value;

        // Obtain user from token
        ApplicationUser user = null;
        if (!string.IsNullOrEmpty(userMailId))
        {
            user = await _userManager.FindByEmailAsync(userMailId);
        }

        return user;
    }

    /// <summary>
    /// Return role of current user
    /// </summary>
    /// <returns>Role of logged in user</returns>
    public async Task<IList<string>> GetCurrentUserRoles()
    {
        ApplicationUser currentUser = await GetCurrentUser();
        IList<string> userRoles = await _userManager.GetRolesAsync(currentUser);

        if (userRoles.Count > 0)
        {
            return userRoles;
        }
        throw new NotFoundException("not found");
    }
    #endregion
}
