using Identity.Application.Interface;
using Identity.Persistance.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

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

            var addUserRole = await _userManager.AddToRolesAsync(user, roles);
            if (!addUserRole.Succeeded)
            {
                var errors = addUserRole.Errors.Select(e => e.Description);
                throw new ValidationException(string.Join("\n", errors));
            }
            return (result.Succeeded, user.Id);
        }
    }
}
