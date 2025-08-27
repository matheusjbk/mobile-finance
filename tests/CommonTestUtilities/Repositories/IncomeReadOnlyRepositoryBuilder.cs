using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Income;
using Moq;

namespace CommonTestUtilities.Repositories;
public class IncomeReadOnlyRepositoryBuilder
{
    private readonly Mock<IIncomeReadOnlyRepository> _mock;

    public IncomeReadOnlyRepositoryBuilder() => _mock = new Mock<IIncomeReadOnlyRepository>();

    public IncomeReadOnlyRepositoryBuilder GetById(User user, Income income)
    {
        _mock.Setup(repo => repo.GetById(user, income.Id)).ReturnsAsync(income);

        return this;
    }

    public IncomeReadOnlyRepositoryBuilder GetByDayOfMonth(User user, IList<Income> incomes)
    {
        _mock.Setup(repo => repo.GetByDayOfMonth(user, It.IsAny<DateTime>())).ReturnsAsync(incomes);

        return this;
    }

    public IncomeReadOnlyRepositoryBuilder GetByPeriod(User user, IList<Income> incomes)
    {
        _mock.Setup(repo => repo.GetByPeriod(user, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(incomes);
        return this;
    }

    public IIncomeReadOnlyRepository Build() => _mock.Object;
}
