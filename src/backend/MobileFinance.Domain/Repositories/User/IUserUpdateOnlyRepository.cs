namespace MobileFinance.Domain.Repositories.User;
public interface IUserUpdateOnlyRepository
{
    public Task<Entities.User?> GetById(long userId);
    public void Update(Entities.User user);
}
