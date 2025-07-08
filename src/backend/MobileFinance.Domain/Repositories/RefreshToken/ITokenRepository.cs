namespace MobileFinance.Domain.Repositories.RefreshToken;
public interface ITokenRepository
{
    public Task<Entities.RefreshToken?> Get(string refreshToken);
    public Task SaveNewRefreshToken(Entities.RefreshToken refreshToken);
}
