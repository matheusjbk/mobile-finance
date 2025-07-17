using MobileFinance.Domain.Services.ServiceBus;
using MobileFinance.Infra.Services.ServiceBus;
using Moq;

namespace CommonTestUtilities.ServiceBus;
public class DeleteUserQueueBuilder
{
    public static IDeleteUserQueue Build() => new Mock<IDeleteUserQueue>().Object;
}
