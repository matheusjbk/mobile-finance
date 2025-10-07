namespace MobileFinance.Domain.Services.Calendar;
public interface IBusinessDayService
{
    public Task<DateTime> GetNextBusinessDay(DateTime date);
    public Task<DateTime> GetNthBusinessDay(int year, int month, int day);
}
