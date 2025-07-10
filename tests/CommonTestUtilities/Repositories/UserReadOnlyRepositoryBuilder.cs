using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _mock;

    public UserReadOnlyRepositoryBuilder() => _mock = new Mock<IUserReadOnlyRepository>();

    public UserReadOnlyRepositoryBuilder ExistActiveUserWithEmail(string email)
    {
        _mock.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(true);

        return this;
    }

    public UserReadOnlyRepositoryBuilder GetByEmail(User? user = null)
    {
        if(user is not null)
            _mock.Setup(repo => repo.GetByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _mock.Object;
}
