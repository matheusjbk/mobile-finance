using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _mock;

    public UserUpdateOnlyRepositoryBuilder() => _mock = new Mock<IUserUpdateOnlyRepository>();

    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _mock.Setup(repo => repo.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }

    public IUserUpdateOnlyRepository Build() => _mock.Object;
}
