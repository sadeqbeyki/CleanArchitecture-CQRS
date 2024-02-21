using AutoMapper;
using System.Text;
using Identity.Application.Common.Exceptions;
using Identity.Application.DTOs;
using Identity.Application.DTOs.AppSettings;
using Identity.Application.DTOs.Auth;
using Identity.Application.Interface;
using Identity.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Identity.Application.Helper;


namespace Identity.Services;

public class IdentityService : ServiceBase<IdentityService>, IIdentityService
{
    private new readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    protected readonly AppSettings _appSettings;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _distributedCache;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,

        AppSettings appSettings,
        IMapper mapper,

        IServiceProvider serviceProvider,
        IConfiguration configuration,
        IDistributedCache distributedCache)
        : base(serviceProvider)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;

        _appSettings = appSettings;
        _mapper = mapper;
        _configuration = configuration;
        _distributedCache = distributedCache;
    }

    #region User
    public async Task<(bool isSucceed, string userId)> CreateUserAsync(RegisterUserDto userDto)
    {
        var existUser = await _userManager.FindByEmailAsync(userDto.Email);
        if (existUser != null)
            throw new BadRequestException($"This email : {existUser.Email} has already been registered in the system!");

        var user = _mapper.Map<ApplicationUser>(userDto);

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new ValidationException(string.Join("\n", errors));
        }

        var addUserRole = await _userManager.AddToRolesAsync(user, new List<string> { "Member" });
        if (!addUserRole.Succeeded)
        {
            var errors = addUserRole.Errors.Select(e => e.Description);
            throw new ValidationException(string.Join("\n", errors));
        }
        return (result.Succeeded, user.Id);
    }
    public async Task<List<UserDetailsDto>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        string key = nameof(GetAllUsersAsync); ;
        List<UserDetailsDto>? members = await _distributedCache.GetObjectAsync<List<UserDetailsDto>>(key, cancellationToken);
        if (members == null)
        {
            var users = await _userManager.Users.ToListAsync();

            members = _mapper.Map<List<UserDetailsDto>>(users);
            await _distributedCache.SetObjectAsync(key, members, _configuration, cancellationToken);

            return members;
        }
        return members;
    }
    public async Task<UserDetailsDto> GetUserAsync(string userId, CancellationToken cancellationToken)
    {
        string key = $"member-{userId}";
        UserDetailsDto? member = await _distributedCache.GetObjectAsync<UserDetailsDto>(key, cancellationToken);
        if (member is null)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");

            member = _mapper.Map<UserDetailsDto>(user);
            member.Roles = await _userManager.GetRolesAsync(user);

            await _distributedCache.SetObjectAsync(key, member, _configuration, cancellationToken);
            return member;
        }
        return member;
    }
    public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.Id)
            ?? throw new NotFoundException("user not found");

        _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new BadRequestException("cant update this user");
        }
        return result.Succeeded;
    }
    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
            ?? throw new NotFoundException("User not found");

        var isUserAdmin = await _userManager.IsInRoleAsync(user, "admin");
        if (isUserAdmin)
        {
            throw new BadRequestException("You can not delete system or admin user");
        }
        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }
    #endregion

    #region MoreUserOptions
    public async Task<string> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        return user == null
            ? throw new NotFoundException("User not found")
            : await _userManager.GetUserNameAsync(user)
             ?? throw new NotFoundException("");
    }
    public async Task<string> GetUserIdAsync(string userName)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == userName)
                ?? throw new NotFoundException("User not found");

        return await _userManager.GetUserIdAsync(user);
    }
    public async Task<UserDetailsDto> GetUserByUserNameAsync(string userName)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName)
            ?? throw new NotFoundException("User not found");
        var userMap = _mapper.Map<UserDetailsDto>(user);

        userMap.Roles = await _userManager.GetRolesAsync(user);
        return userMap;
    }
    public async Task<bool> IsUniqueUserName(string userName)
    {
        return await _userManager.FindByNameAsync(userName) == null;
    }
    #endregion

    #region Role
    public async Task<bool> CreateRoleAsync(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new ValidationException(string.Join("\n", errors));
        }
        return result.Succeeded;
    }
    public async Task<List<(string id, string? roleName)>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.Select(x => new
        {
            x.Id,
            x.Name
        }).ToListAsync();

        return roles.Select(role => (role.Id, role.Name)).ToList();
    }
    public async Task<(string id, string roleName)> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id)
            ?? throw new NotFoundException($"cant find any role with this id : {id}");
        return (role.Id, role.Name);
    }
    public async Task<bool> UpdateRole(string id, string roleName)
    {
        if (roleName != null)
        {
            var role = await _roleManager.FindByIdAsync(id)
                ?? throw new NotFoundException();
            role.Name = roleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        return false;
    }
    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        var roleDetails = await _roleManager.FindByIdAsync(roleId)
            ?? throw new NotFoundException("Role not found");
        if (roleDetails.Name == "Admin")
        {
            throw new BadRequestException("You can not delete Administrator Role");
        }
        var result = await _roleManager.DeleteAsync(roleDetails);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new ValidationException(string.Join("\n", errors));
        }
        return result.Succeeded;
    }
    #endregion

    #region UserRole
    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        return await _userManager.IsInRoleAsync(user, role);
    }
    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }
    public async Task<bool> AssignUserToRole(string userName, IList<string> roles)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName)
            ?? throw new NotFoundException("User not found");

        var result = await _userManager.AddToRolesAsync(user, roles);
        return result.Succeeded;
    }
    public async Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
    {
        var user = await _userManager.FindByNameAsync(userName)
            ?? throw new NotFoundException("cant find user"); ;
        var existingRoles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, existingRoles);
        result = await _userManager.AddToRolesAsync(user, usersRole);

        return result.Succeeded;
    }
    #endregion

    #region Account
    public async Task<JwtTokenDto> SigninUserAsync(LoginUserDto request)
    {
        var user = await _userManager.FindByNameAsync(request.Username)
            ?? throw new BadRequestException("User Doesn't exist!");

        if (user.UserName == request.Username)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(request.Username, request.Password, true, false);
            if (!loginResult.Succeeded)
            {
                throw new NotFoundException("user is not allowed");
            }
            //await _signInManager.SignInAsync(user, isPersistent: false);
            var result = await GenerateJWTAuthetication(user);
            return result;
        }
        throw new BadRequestException("User Not Found!");
    }

    #endregion

    #region private extention   

    /// <summary>
    /// Generate JWT token for user
    /// </summary>
    /// <param name="user">User entity for which token will be generate</param>
    /// <returns>Generated token details including expire data</returns>
    public async Task<JwtTokenDto> GetJwtSecurityTokenAsync(ApplicationUser user)
    {
        var jwtOptions = _appSettings.JwtIssuerOptions;

        // Obtain existing claims, Here we will obtain last 4 JTI claims only
        // As We only maintain login for 5 maximum sessions, So need to remove other from that
        var allClaims = await _userManager.GetClaimsAsync(user);
        var toRemoveClaims = new List<Claim>();
        var allJtiClaims = allClaims.Where(claim => claim.Type.Equals(JwtRegisteredClaimNames.Jti)).ToList();
        if (allJtiClaims.Count > 4)
        {
            toRemoveClaims = allJtiClaims.SkipLast(4).ToList();
            allJtiClaims = allJtiClaims.TakeLast(4).ToList();
        }

        var secretKey = _configuration["JwtIssuerOptions:SecretKey"];

        SigningCredentials credentials = new(new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

        DateTime tokenExpireOn = DateTime.Now.AddHours(3);
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?
            .Equals("Development", StringComparison.InvariantCultureIgnoreCase) == true)
        {
            // If its development then set 3 years as token expiry for testing purpose
            tokenExpireOn = DateTime.Now.AddYears(3);
        }

        string roles = string.Join("; ", await _userManager.GetRolesAsync(user));

        // Obtain Role of User
        IList<string> rolesOfUser = await _userManager.GetRolesAsync(user);

        // Add new claims
        string userName = user.UserName ?? throw new NotFoundException("cant find user");
        List<Claim> tokenClaims =
        [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Role, rolesOfUser.FirstOrDefault() ?? "Member"),
            new(ClaimTypes.Name, user.PhoneNumber)
        ];

        // Make JWT token
        JwtSecurityToken token = new(
            issuer: jwtOptions.Issuer,/* _config("JwtIssuerOptions:Issuer"),*/
            audience: jwtOptions.Audience, /*_config("JwtIssuerOptions:Audience"),*/
            claims: tokenClaims.Union(allJtiClaims),
            expires: tokenExpireOn,
            signingCredentials: credentials
        );

        // Update claim details
        await _userManager.RemoveClaimsAsync(user, toRemoveClaims);
        /*var claims =*/
        await _userManager.AddClaimsAsync(user, tokenClaims);

        // Return it
        JwtTokenDto generatedToken = new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireOn = tokenExpireOn,
            UserDetails = await GetUserDetailsAsync(user),
        };

        return generatedToken;
    }

    public async Task<UserDetailsDto> GetUserDetailsAsync(ApplicationUser user)
    {
        if (user != null)
        {
            var userDetails = _mapper.Map<UserDetailsDto>(user);
            userDetails.Roles = await _userManager.GetRolesAsync(user);
            return userDetails;
        }

        throw new NotFoundException("User not found");
    }

    public async Task<ApplicationUser> GetUserByNameAsync(string userName)
    {
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
                return user;
            throw new BadRequestException("user not found");
        }
        throw new BadRequestException("cant search for nul value!");
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user ?? throw new NotFoundException("cant find user");
    }

    private async Task<JwtTokenDto> GenerateJWTAuthetication(ApplicationUser user)
    {
        var claims = await GetClaimsIdentity(user);

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtIssuerOptions:SecretKey"]));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
        var expires = DateTime.Now.AddHours(8);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtIssuerOptions:Issuer"],
            audience: _configuration["JwtIssuerOptions:Audience"],
            claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return new JwtTokenDto()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpireOn = expires,
            UserDetails = await GetUserDetailsAsync(user),
        };
    }

    private async Task<IEnumerable<Claim>> GetClaimsIdentity(ApplicationUser user)
    {
        IList<string> rolesOfUser = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>();
        foreach (var role in rolesOfUser)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        claims.Add(new(JwtHeaderParameterNames.Jku, user.UserName));
        claims.Add(new(JwtHeaderParameterNames.Kid, Guid.NewGuid().ToString()));

        return claims;
    }
    #endregion
}
