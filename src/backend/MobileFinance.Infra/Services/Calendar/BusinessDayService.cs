using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Services.Calendar;
using MobileFinance.Infra.DataAccess;

namespace MobileFinance.Infra.Services.Calendar;
public class BusinessDayService : IBusinessDayService
{
    private readonly MobileFinanceDbContext _dbContext;

    public BusinessDayService(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task<DateTime> GetNextBusinessDay(DateTime date)
    {
        var nextDate = date;

        while(true)
        {
            var calendarDay = await _dbContext.CalendarDays.FirstOrDefaultAsync(d => d.Date == nextDate.Date);

            if(calendarDay!.IsBusinessDay)
                return nextDate;

            nextDate = nextDate.AddDays(1);
        }
    }

    public async Task<DateTime> GetNthBusinessDay(int year, int month, int day)
    {
        if(day <= 0)
            throw new Exception();

        var monthStart = new DateTime(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        var businessDates = await _dbContext.CalendarDays
            .AsNoTracking()
            .Where(d => d.Date >= monthStart && d.Date <= monthEnd && d.IsBusinessDay)
            .OrderBy(d => d.Date)
            .Select(d => d.Date)
            .ToListAsync();

        if(businessDates.Count.Equals(0))
            throw new Exception();

        if(day <= businessDates.Count)
            return businessDates[day - 1];

        return businessDates.Last();
    }
}
