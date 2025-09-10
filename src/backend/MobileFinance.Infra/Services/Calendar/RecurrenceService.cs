using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Enums;
using MobileFinance.Domain.Services.Calendar;

namespace MobileFinance.Infra.Services.Calendar;
public class RecurrenceService : IRecurrenceService
{
    public IEnumerable<(DateTime date, Income income)> GetOcurrences(Income income, DateTime start, DateTime end)
    {
        var occurrences = new List<(DateTime, Income)>();

        if(income.IncomeType == IncomeType.OneTime)
        {
             if (income.ReceivedOn >= start && income.ReceivedOn <= end)
                occurrences.Add((income.ReceivedOn, income));

            return occurrences;
        }

        var currentDate = income.ReceivedOn;
        var count = 0;

        while(currentDate <= end)
        {
            if(income.RecurrenceMonthsCount.HasValue && count >= income.RecurrenceMonthsCount.Value)
                break;

            if(currentDate >= start)
                occurrences.Add((currentDate, income));

            currentDate = currentDate.AddMonths(1);
            count++;
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
