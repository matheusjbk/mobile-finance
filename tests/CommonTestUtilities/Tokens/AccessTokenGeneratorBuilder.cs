using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Infra.Security.Tokens.AccessToken.Generator;

namespace CommonTestUtilities.Tokens;
public class AccessTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtGenerator(5, "tttttttttttttttttttttttttttttttt");
}
