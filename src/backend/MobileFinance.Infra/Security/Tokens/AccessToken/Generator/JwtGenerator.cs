using Microsoft.IdentityModel.Tokens;
using MobileFinance.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MobileFinance.Infra.Security.Tokens.AccessToken.Generator;
public class JwtGenerator : JwtHandler, IAccessTokenGenerator
{
    private readonly uint _expirationTimeInMinutes;
    private readonly string _signingKey;

    public JwtGenerator(uint expirationTimeInMinutes, string signingKey)
    {
        _expirationTimeInMinutes = expirationTimeInMinutes;
        _signingKey = signingKey;
    }

    public string Generate(Guid userIdentifier)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Sid, userIdentifier.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(
                SecurityKey(_signingKey),
                SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
