using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Services;
using Moq;

namespace CommonTestUtilities.LoggedUser;
public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(x => x.GetUser()).ReturnsAsync(user);

        return mock.Object;
    }
}
