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
    private readonly IBusinessDayService _businessDayService;

    public GetCalendarDayUseCase(
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

    public async Task<ResponseCalendarDayJson> Execute(DateTime date)
    {
        var loggedUser = await _loggedUser.GetUser();

        var endDate = date.AddDays(1).AddTicks(-1);

        var incomes = await _incomeReadOnlyRepository.GetByPeriod(loggedUser, date, endDate);
        var incomeOccurrences = new List<(DateTime, Domain.Entities.Income)>();
        foreach(var income in incomes)
        {
            var occurrences = await _recurrenceService.GetOcurrences(income, date, endDate);

            foreach(var occurrence in occurrences)
            {
                var shifted = await _businessDayService.GetNextBusinessDay(occurrence.date.Date);
                incomeOccurrences.Add((shifted, occurrence.income));
            }
        }

        var debits = await _debitReadOnlyRepository.GetByPeriod(loggedUser, date, endDate);
        var debitOccurrences = new List<(DateTime, Domain.Entities.Debit)>();
        foreach(var debit in debits)
        {
            var occurrences = _recurrenceService.GetOcurrences(debit, date, endDate);
            foreach(var occurrence in occurrences)
            {
                var shifted = await _businessDayService.GetNextBusinessDay(occurrence.date.Date);
                debitOccurrences.Add((shifted, occurrence.debit));
            }
        }

        var dayIncomes = incomeOccurrences
                .Where(occurrence => occurrence.Item1.Date == date.Date)
                .Select(occurrence => occurrence.Item2.Adapt<ResponseShortIncomeJson>())
                .ToList();

        var dayDebits = debitOccurrences
            .Where(occurrence => occurrence.Item1.Date == date.Date)
            .Select(occurrence => occurrence.Item2.Adapt<ResponseShortDebitJson>())
            .ToList();

        return new ResponseCalendarDayJson
        {
            Date = date,
            Incomes = dayIncomes,
            Debits = dayDebits
        };
    }
}
