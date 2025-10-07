using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Enums;
using MobileFinance.Domain.Services.Calendar;

namespace MobileFinance.Infra.Services.Calendar;
public class RecurrenceService : IRecurrenceService
{
    private readonly IBusinessDayService _businessDayService;

    public RecurrenceService(IBusinessDayService businessDayService) => _businessDayService = businessDayService;

    public async Task<IEnumerable<(DateTime date, Income income)>> GetOcurrences(Income income, DateTime start, DateTime end)
    {
        var occurrences = new List<(DateTime, Income)>();

        if(income.IncomeType == IncomeType.OneTime)
        {
             if (income.ReceivedOn.HasValue && income.ReceivedOn.Value >= start && income.ReceivedOn.Value <= end)
                occurrences.Add((income.ReceivedOn.Value, income));

            return occurrences;
        }

        if(income.IncomeType == IncomeType.Rent)
        {
            if(!income.ReceivedOn.HasValue) return occurrences;

            var currentDate = income.ReceivedOn!.Value;

            while(currentDate <= end)
            {
                if(currentDate >= start) occurrences.Add((currentDate, income));
                currentDate = currentDate.AddMonths(1);
            }

            return occurrences;
        }

        if(income.IncomeType == IncomeType.Salary)
        {
            if(!income.BusinessDayNumber.HasValue) return occurrences;

            var cursor = new DateTime(start.Year, start.Month, 1);
            var endMonth = new DateTime(end.Year, end.Month, 1);

            while(cursor <= endMonth)
            {
                var salaryDate = await _businessDayService.GetNthBusinessDay(cursor.Year, cursor.Month, income.BusinessDayNumber.Value);

                if(salaryDate >= start && salaryDate <= end)
                    occurrences.Add((salaryDate, income));

                cursor = cursor.AddMonths(1);
            }

            return occurrences;
        }

        return occurrences;
    }
    public IEnumerable<(DateTime date, Debit debit)> GetOcurrences(Debit debit, DateTime start, DateTime end)
    {
        var occurrences = new List<(DateTime, Debit)>();

        if(debit.DebitType == DebitType.OneTime)
        {
            if(debit.PaidOn >= start && debit.PaidOn <= end)
                occurrences.Add((debit.PaidOn, debit));

            return occurrences;
        }

        var currentDate = debit.PaidOn;
        var count = 0;

        while(currentDate <= end)
        {
            if(debit.RecurrenceMonthsCount.HasValue && count >= debit.RecurrenceMonthsCount.Value)
                break;

            if(currentDate >= start)
                occurrences.Add((currentDate, debit));

            currentDate = currentDate.AddMonths(1);
            count++;
        }

        return occurrences;
    }
}
