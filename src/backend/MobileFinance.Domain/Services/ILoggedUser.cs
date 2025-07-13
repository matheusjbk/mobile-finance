using MobileFinance.Domain.Entities;

namespace MobileFinance.Domain.Services;
public interface ILoggedUser
{
    public Task<User> GetUser();
}
