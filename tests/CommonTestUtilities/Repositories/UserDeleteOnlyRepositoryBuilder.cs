using MobileFinance.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserDeleteOnlyRepositoryBuilder
{
    public static IUserDeleteOnlyRepository Build() => new Mock<IUserDeleteOnlyRepository>().Object;
}
