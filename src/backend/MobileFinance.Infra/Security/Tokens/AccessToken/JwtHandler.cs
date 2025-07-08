using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MobileFinance.Infra.Security.Tokens.AccessToken;
public abstract class JwtHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);

        return new SymmetricSecurityKey(bytes);
    }
}
