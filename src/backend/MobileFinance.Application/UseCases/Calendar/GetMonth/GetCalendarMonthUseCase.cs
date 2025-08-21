using Mapster;
using MobileFinance.Communication.Responses;
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

    public GetCalendarMonthUseCase(
        ILoggedUser loggedUser,
        IRecurrenceService recurrenceService,
        IIncomeReadOnlyRepository incomeReadOnlyRepository,
        IDebitReadOnlyRepository debitReadOnlyRepository)
    {
        _loggedUser = loggedUser;
        _recurrenceService = recurrenceService;
        _incomeReadOnlyRepository = incomeReadOnlyRepository;
        _debitReadOnlyRepository = debitReadOnlyRepository;
    }

    public async Task<ResponseCalendarMonthJson> Execute(int year, int month)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1).AddDays(-1);

        var loggedUser = await _loggedUser.GetUser();

        var incomes = await _incomeReadOnlyRepository.GetByPeriod(loggedUser, start, end);
        var debits = await _debitReadOnlyRepository.GetByPeriod(loggedUser, start, end);

        var incomeOccurrences = incomes.SelectMany(i => _recurrenceService.GetOcurrences(i, start, end)).ToList();
        var debitOccurrences = debits.SelectMany(d => _recurrenceService.GetOcurrences(d, start, end)).ToList();

        var days = new List<ResponseCalendarDayJson>();

        for(var date = start; date <= end; date = date.AddDays(1))
        {
            var dayIncomes = incomeOccurrences
                .Where(occurrence => occurrence.date.Date == date.Date)
                .Select(occurrence => occurrence.income.Adapt<ResponseShortIncomeJson>())
                .ToList();

            var dayDebits = debitOccurrences
                .Where(occurrence => occurrence.date.Date == date.Date)
                .Select(occurrence => occurrence.debit.Adapt<ResponseShortDebitJson>())
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
