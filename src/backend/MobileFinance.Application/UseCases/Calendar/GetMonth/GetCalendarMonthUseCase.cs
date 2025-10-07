using Mapster;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Debit;
using MobileFinance.Domain.Repositories.Income;
using MobileFinance.Domain.Services.Calendar;
using MobileFinance.Domain.Services.LoggedUser;

namespace MobileFinance.Application.UseCases.Calendar.GetMonth;
public class GetCalendarMonthUseCase : IGetCalendarMonthUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecurrenceService _recurrenceService;
    private readonly IIncomeReadOnlyRepository _incomeReadOnlyRepository;
    private readonly IDebitReadOnlyRepository _debitReadOnlyRepository;
    private readonly IBusinessDayService _businessDayService;

    public GetCalendarMonthUseCase(
        ILoggedUser loggedUser,
        IRecurrenceService recurrenceService,
        IIncomeReadOnlyRepository incomeReadOnlyRepository,
        IDebitReadOnlyRepository debitReadOnlyRepository,
        IBusinessDayService businessDayService)
    {
        _loggedUser = loggedUser;
        _recurrenceService = recurrenceService;
        _incomeReadOnlyRepository = incomeReadOnlyRepository;
        _debitReadOnlyRepository = debitReadOnlyRepository;
        _businessDayService = businessDayService;
    }

    public async Task<ResponseCalendarMonthJson> Execute(int year, int month)
    {
        var start = new DateTime(year, month, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc);
        var end = start.AddMonths(1).AddTicks(-1);

        var loggedUser = await _loggedUser.GetUser();

        var incomes = await _incomeReadOnlyRepository.GetByPeriod(loggedUser, start, end);
        var incomeOccurrences = new List<(DateTime, Domain.Entities.Income)>();
        foreach(var income in incomes)
        {
            var occurrences = await _recurrenceService.GetOcurrences(income, start, end);

            foreach(var occurrence in occurrences)
            {
                var shifted = await _businessDayService.GetNextBusinessDay(occurrence.date.Date);
                incomeOccurrences.Add((shifted, occurrence.income));
            }
        }

        var debits = await _debitReadOnlyRepository.GetByPeriod(loggedUser, start, end);
        var debitOccurrences = new List<(DateTime, Domain.Entities.Debit)>();
        foreach(var debit in debits)
        {
            var occurrences = _recurrenceService.GetOcurrences(debit, start, end);
            foreach(var occurrence in occurrences)
            {
                var shifted = await _businessDayService.GetNextBusinessDay(occurrence.date.Date);
                debitOccurrences.Add((shifted, occurrence.debit));
            }
        }

        var days = new List<ResponseCalendarDayJson>();

        for(var date = start; date <= end; date = date.AddDays(1))
        {
            var dayIncomes = incomeOccurrences
                .Where(occurrence => occurrence.Item1 == date.Date)
                .Select(occurrence => occurrence.Item2.Adapt<ResponseShortIncomeJson>())
                .ToList();

            var dayDebits = debitOccurrences
                .Where(occurrence => occurrence.Item1 == date.Date)
                .Select(occurrence => occurrence.Item2.Adapt<ResponseShortDebitJson>())
                .ToList();

            days.Add(new ResponseCalendarDayJson
            {
                Date = date,
                Incomes = dayIncomes,
                Debits = dayDebits
            });
        }

        return new ResponseCalendarMonthJson
        {
            Year = year,
            Month = month,
            Days = days
        };
    }
}
