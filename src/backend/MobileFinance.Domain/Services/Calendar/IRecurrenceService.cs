using MobileFinance.Domain.Entities;

namespace MobileFinance.Domain.Services.Calendar;
public interface IRecurrenceService
{
    public IEnumerable<(DateTime date, Income income)> GetOcurrences(Income income, DateTime start, DateTime end);
    public IEnumerable<(DateTime date, Debit debit)> GetOcurrences(Debit debit, DateTime start, DateTime end);
}
