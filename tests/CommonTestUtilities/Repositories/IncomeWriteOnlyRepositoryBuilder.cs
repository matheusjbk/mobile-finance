using MobileFinance.Domain.Repositories.Income;
using Moq;

namespace CommonTestUtilities.Repositories;
public class IncomeWriteOnlyRepositoryBuilder
{
    public static IIncomeWriteOnlyRepository Build() => new Mock<IIncomeWriteOnlyRepository>().Object;
}
