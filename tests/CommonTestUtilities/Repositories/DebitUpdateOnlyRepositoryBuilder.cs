using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Debit;
using Moq;

namespace CommonTestUtilities.Repositories;
public class DebitUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IDebitUpdateOnlyRepository> _mock;

    public DebitUpdateOnlyRepositoryBuilder() => _mock = new Mock<IDebitUpdateOnlyRepository>();

    public DebitUpdateOnlyRepositoryBuilder GetById(User user, Debit debit)
    {
        _mock.Setup(repo => repo.GetById(user, debit.Id)).ReturnsAsync(debit);

        return this;
    }

    public IDebitUpdateOnlyRepository Build() => _mock.Object;
}
