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

    public IIncomeReadOnlyRepository Build() => _mock.Object;
}
