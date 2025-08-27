using Mapster;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories.Debit;
using MobileFinance.Domain.Repositories.Income;
using MobileFinance.Domain.Services.Calendar;
using MobileFinance.Domain.Services.LoggedUser;

namespace MobileFinance.Application.UseCases.Calendar.GetDay;
public class GetCalendarDayUseCase : IGetCalendarDayUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRecurrenceService _recurrenceService;
    private readonly IIncomeReadOnlyRepository _incomeReadOnlyRepository;
    private readonly IDebitReadOnlyRepository _debitReadOnlyRepository;

    public GetCalendarDayUseCase(
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

    public async Task<ResponseCalendarDayJson> Execute(DateTime date)
    {
        var loggedUser = await _loggedUser.GetUser();

        var incomes = await _incomeReadOnlyRepository.GetByDayOfMonth(loggedUser, date);
        var debits = await _debitReadOnlyRepository.GetByDayOfMonth(loggedUser, date);

        var endDate = date.AddDays(1).AddTicks(-1);

        var incomeOccurrences = incomes.SelectMany(i => _recurrenceService.GetOcurrences(i, date, endDate)).ToList();
        var debitOccurrences = debits.SelectMany(d => _recurrenceService.GetOcurrences(d, date, endDate)).ToList();

        var dayIncomes = incomeOccurrences
                .Where(occurrence => occurrence.date.Date == date.Date)
                .Select(occurrence => occurrence.income.Adapt<ResponseShortIncomeJson>())
                .ToList();

        var dayDebits = debitOccurrences
            .Where(occurrence => occurrence.date.Date == date.Date)
            .Select(occurrence => occurrence.debit.Adapt<ResponseShortDebitJson>())
            .ToList();

        return new ResponseCalendarDayJson
        {
            Date = date,
            Incomes = dayIncomes,
            Debits = dayDebits
        };
    }
}
