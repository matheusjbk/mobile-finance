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
}
