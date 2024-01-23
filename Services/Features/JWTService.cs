using Application.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Services.Features;

public class JWTService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string ValidateToken(string token)
    {
        if (token == null)
            return "Invalid token: Token is null.";

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtIssuerOptions:SecretKey")));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero

            }, out SecurityToken validatedToken);

            // Corrected access to the validatedToken
            var jwtToken = (JwtSecurityToken)validatedToken;
            var jku = jwtToken.Claims.First(claim => claim.Type == "jku").Value;
            var kid = jwtToken.Claims.First(claim => claim.Type == "kid").Value;

            return kid;
        }
        catch (Exception ex)
        {
            return $"Invalid token: {ex.Message}";
        }
    }
}
