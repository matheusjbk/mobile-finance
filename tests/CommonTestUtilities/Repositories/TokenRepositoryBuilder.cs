using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.RefreshToken;
using Moq;

namespace CommonTestUtilities.Repositories;
public class TokenRepositoryBuilder
{
    private readonly Mock<ITokenRepository> _mock;

    public TokenRepositoryBuilder() => _mock = new Mock<ITokenRepository>();

    public TokenRepositoryBuilder Get(RefreshToken refreshToken)
    {
        _mock.Setup(repo => repo.Get(refreshToken.Value)).ReturnsAsync(refreshToken);

        return this;
    }

    public ITokenRepository Build() => _mock.Object;
}
