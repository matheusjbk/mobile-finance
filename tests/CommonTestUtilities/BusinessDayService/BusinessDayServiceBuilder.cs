using MobileFinance.Domain.Services.Calendar;
using Moq;

namespace CommonTestUtilities.BusinessDayService;
public class BusinessDayServiceBuilder
{
    private readonly Mock<IBusinessDayService> _mock;

    public BusinessDayServiceBuilder() => _mock = new Mock<IBusinessDayService>();

    public BusinessDayServiceBuilder GetNextBusinessDay(DateTime date, DateTime returnDate)
    {
        _mock.Setup(service => service.GetNextBusinessDay(date)).ReturnsAsync(returnDate);

        return this;
    }

    public BusinessDayServiceBuilder GetNthBusinessDay(DateTime date, DateTime returnDate)
    {
        _mock.Setup(service => service.GetNthBusinessDay(date.Year, date.Month, date.Day)).ReturnsAsync(returnDate);

        return this;
    }

    public IBusinessDayService Build() => _mock.Object;
}
