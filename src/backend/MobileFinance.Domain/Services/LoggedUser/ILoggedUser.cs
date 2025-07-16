using MobileFinance.Domain.Entities;

namespace MobileFinance.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    public Task<User> GetUser();
}
