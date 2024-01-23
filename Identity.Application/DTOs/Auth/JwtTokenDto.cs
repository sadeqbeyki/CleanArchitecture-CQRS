using Identity.Persistance.Identity;

namespace Identity.Application.DTOs.Auth
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
        public ApplicationUser User { get; set; }
        public UserDetailsDto UserDetails { get; set; }
    }
}
