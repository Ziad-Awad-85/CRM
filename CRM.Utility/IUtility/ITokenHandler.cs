using System.Security.Claims;

namespace CRM.Utility.IUtility
{
    public interface ITokenHandler
    {
        string GenerateJwtToken(List<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        int GetRefreshTokenExpiryDays();
        int GetMaxRefreshTokenAttempts();

    }
}
