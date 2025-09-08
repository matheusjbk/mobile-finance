namespace MobileFinance.Domain.Services.Calendar;
public interface IBusinessDayService
{
    public Task<DateTime> GetNextBusinessDay(DateTime date);
}
