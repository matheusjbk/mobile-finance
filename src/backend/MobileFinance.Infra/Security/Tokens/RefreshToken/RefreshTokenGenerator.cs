using MobileFinance.Domain.Security.Tokens;

namespace MobileFinance.Infra.Security.Tokens.RefreshToken;
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
}
