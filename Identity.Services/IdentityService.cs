using AutoMapper;
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


namespace Identity.Services
{
    public class IdentityService : ServiceBase<IdentityService>, IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        protected readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,

            AppSettings appSettings,
            IMapper mapper,

            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            _appSettings = appSettings;
            _mapper = mapper;
        }

        #region User
        public async Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string fullName, List<string> roles)
        {
            var user = new ApplicationUser()
            {
                FullName = fullName,
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new ValidationException(string.Join("\n", errors));
            }

            if (roles == null 
                || !roles.Any()
                || roles.All(string.IsNullOrWhiteSpace)
                || roles.Contains("string"))
                roles = new List<string> { "Member" };

            var addUserRole = await _userManager.AddToRolesAsync(user, roles);
            if (!addUserRole.Succeeded)
            {
                var errors = addUserRole.Errors.Select(e => e.Description);
                throw new ValidationException(string.Join("\n", errors));
            }
            return (result.Succeeded, user.Id);
        }
        public async Task<List<(string id, string fullName, string userName, string email)>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.Select(x => new
            {
                x.Id,
                x.FullName,
                x.UserName,
                x.Email
            }).ToListAsync();

            return users.Select(user => (user.Id, user.FullName, user.UserName, user.Email)).ToList();
        }
        public async Task<(string userId, string fullName, string userName, string email, IList<string> roles)> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.FullName, user.UserName, user.Email, roles);
        }
        public async Task<bool> UpdateUser(string id, string fullName, string email, IList<string> roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.FullName = fullName;
            user.Email = email;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }

            if (user.UserName == "system" || user.UserName == "admin")
            {
                throw new Exception("You can not delete system or admin user");
                //throw new BadRequestException("You can not delete system or admin user");
            }
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        #endregion

        #region MoreUserOptions
        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
                //throw new Exception("User not found");
            }
            return await _userManager.GetUserNameAsync(user);
        }
        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return await _userManager.GetUserIdAsync(user);
        }
        public async Task<(string userId, string fullName, string userName, string email, IList<string> roles)> GetUserDetailsByUserNameAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.FullName, user.UserName, user.Email, roles);
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
        public async Task<List<(string id, string roleName)>> GetRolesAsync()
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
            var role = await _roleManager.FindByIdAsync(id);
            return (role.Id, role.Name);
        }
        public async Task<bool> UpdateRole(string id, string roleName)
        {
            if (roleName != null)
            {
                var role = await _roleManager.FindByIdAsync(id);
                role.Name = roleName;
                var result = await _roleManager.UpdateAsync(role);
                return result.Succeeded;
            }
            return false;
        }
        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var roleDetails = await _roleManager.FindByIdAsync(roleId);
            if (roleDetails == null)
            {
                throw new NotFoundException("Role not found");
            }

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
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }
        public async Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var existingRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, existingRoles);
            result = await _userManager.AddToRolesAsync(user, usersRole);

            return result.Succeeded;
        }
        #endregion

        #region Account
        public async Task<bool> SigninUserAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            if (!result.Succeeded)
            {
                throw new NotFoundException("user is not allowed");
            }
            return result.Succeeded;
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

            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

            DateTime tokenExpireOn = DateTime.Now.AddHours(3);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Development", StringComparison.InvariantCultureIgnoreCase) == true)
            {
                // If its development then set 3 years as token expiry for testing purpose
                tokenExpireOn = DateTime.Now.AddYears(3);
            }

            string roles = string.Join("; ", await _userManager.GetRolesAsync(user));

            // Obtain Role of User
            IList<string> rolesOfUser = await _userManager.GetRolesAsync(user);

            // Add new claims
            List<Claim> tokenClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, rolesOfUser.FirstOrDefault() ?? "Member"),
            };

            // Make JWT token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: tokenClaims.Union(allJtiClaims),
                expires: tokenExpireOn,
                signingCredentials: credentials
            );
            //_logger.LogDebug($"Token generated. UserId: {user.Email}");

            // Set current user details for busines & common library
            var currentUser = await _userManager.FindByEmailAsync(user.Email);

            // Update claim details
            await _userManager.RemoveClaimsAsync(currentUser, toRemoveClaims);
            await _userManager.AddClaimsAsync(currentUser, tokenClaims);

            // Return it
            JwtTokenDto generatedToken = new JwtTokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireOn = tokenExpireOn,
                UserDetails = await GetUserDetailsAsync(currentUser)
            };

            return generatedToken;
        }

        public async Task<UserDetailsDto> GetUserDetailsAsync(ApplicationUser user)
        {
            if (user != null)
            {
                var userDetails = _mapper.Map<UserDetailsDto>(user);

                var currentUserRoles = await _userManager.GetRolesAsync(user);
                userDetails.Role = currentUserRoles.FirstOrDefault() ?? string.Empty;
                return userDetails;
            }

            return null;
        }

        public async Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                return user;
            }
            return null;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user ?? throw new NotFoundException("cant find user");
        }
        #endregion
    }
}
