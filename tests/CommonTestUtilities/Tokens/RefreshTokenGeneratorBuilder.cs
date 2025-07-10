using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Infra.Security.Tokens.RefreshToken;

namespace CommonTestUtilities.Tokens;
public class RefreshTokenGeneratorBuilder
{
    public static IRefreshTokenGenerator Build() => new RefreshTokenGenerator();
}
