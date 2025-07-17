namespace MobileFinance.Domain.Repositories.User;
public interface IUserDeleteOnlyRepository
{
    public Task DeleteAccount(Guid userIdentifier);
}
