using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinancialApplication.Helpers;

public class JWTHelper : IJWTHelper
{
    private readonly IConfiguration _configuration;
    public JWTHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public JwtSecurityToken GenerateToken(ApplicationUser user, IList<string> userRoles)
    {
        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        return new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
    }
}
