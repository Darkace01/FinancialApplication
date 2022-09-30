using System.IdentityModel.Tokens.Jwt;

namespace FinancialApplication.Helpers
{
    public interface IJWTHelper
    {
        JwtSecurityToken GenerateToken(ApplicationUser user, IList<string> userRoles);
    }
}