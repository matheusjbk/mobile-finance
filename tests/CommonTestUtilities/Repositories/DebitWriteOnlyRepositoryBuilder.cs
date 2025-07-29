using MobileFinance.Domain.Repositories.Debit;
using Moq;

namespace CommonTestUtilities.Repositories;
public class DebitWriteOnlyRepositoryBuilder
{
    public static IDebitWriteOnlyRepository Build() => new Mock<IDebitWriteOnlyRepository>().Object;
}
