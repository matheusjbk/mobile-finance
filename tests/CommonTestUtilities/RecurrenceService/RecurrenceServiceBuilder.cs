using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Services.Calendar;
using Moq;

namespace CommonTestUtilities.RecurrenceService;
public class RecurrenceServiceBuilder
{
    private readonly Mock<IRecurrenceService> _mock;

    public RecurrenceServiceBuilder() => _mock = new Mock<IRecurrenceService>();

    public RecurrenceServiceBuilder GetOcurrences(IList<Income> incomes)
    {
        foreach(var income in incomes)
            _mock.Setup(repo => repo.GetOcurrences(income, It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<(DateTime, Income)> { (income.ReceivedOn!.Value, income) });

        return this;
    }

    public RecurrenceServiceBuilder GetOcurrences(IList<Debit> debits)
    {
        foreach(var debit in debits)
            _mock.Setup(repo => repo.GetOcurrences(debit, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<(DateTime, Debit)> { (debit.PaidOn, debit) });

        return this;
    }

    public IRecurrenceService Build() => _mock.Object;
}
