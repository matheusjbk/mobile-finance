using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Income;
using Moq;

namespace CommonTestUtilities.Repositories;
public class IncomeUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IIncomeUpdateOnlyRepository> _mock;

    public IncomeUpdateOnlyRepositoryBuilder() => _mock = new Mock<IIncomeUpdateOnlyRepository>();

    public IncomeUpdateOnlyRepositoryBuilder GetById(User user, Income income)
    {
        _mock.Setup(repo => repo.GetById(user, income.Id)).ReturnsAsync(income);

        return this;
    }

    public IIncomeUpdateOnlyRepository Build() => _mock.Object;
}
