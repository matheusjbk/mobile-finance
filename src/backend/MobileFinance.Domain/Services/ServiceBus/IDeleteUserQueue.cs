using MobileFinance.Domain.Entities;

namespace MobileFinance.Domain.Services.ServiceBus;
public interface IDeleteUserQueue
{
    public Task SendMessage(User user);
}
