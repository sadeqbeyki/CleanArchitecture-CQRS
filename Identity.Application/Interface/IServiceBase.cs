using Identity.Persistance.Identity;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Interface
{
    public interface IServiceBase
    {
        string GetCurrentUserId();
        Task<ApplicationUser> GetCurrentUser();
        Task<IList<string>> GetCurrentUserRoles();
    }
}
