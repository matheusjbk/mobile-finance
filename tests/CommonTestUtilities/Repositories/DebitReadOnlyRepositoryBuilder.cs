using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Debit;
using Moq;

namespace CommonTestUtilities.Repositories;
public class DebitReadOnlyRepositoryBuilder
{
    private readonly Mock<IDebitReadOnlyRepository> _mock;

    public DebitReadOnlyRepositoryBuilder() => _mock = new Mock<IDebitReadOnlyRepository>();

    public DebitReadOnlyRepositoryBuilder GetById(User user, Debit debit)
    {
        _mock.Setup(repo => repo.GetById(user, debit.Id)).ReturnsAsync(debit);

        return this;
    }

    public DebitReadOnlyRepositoryBuilder GetByDayOfMonth(User user, IList<Debit> debits)
    {
        _mock.Setup(repo => repo.GetByDayOfMonth(user, It.IsAny<DateTime>())).ReturnsAsync(debits);

        return this;
    }

    public IDebitReadOnlyRepository Build() => _mock.Object;
}
