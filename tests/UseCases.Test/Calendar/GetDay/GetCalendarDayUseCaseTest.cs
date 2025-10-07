using CommonTestUtilities.BusinessDayService;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.RecurrenceService;
using CommonTestUtilities.Repositories;
using MobileFinance.Application.UseCases.Calendar.GetDay;
using Shouldly;

namespace UseCases.Test.Calendar.GetDay;
public class GetCalendarDayUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var date = DateTime.UtcNow.Date;
        DateTime returnDate;
        while(true)
        {
            var scopeDate = date;
            if(!(scopeDate.DayOfWeek == DayOfWeek.Sunday) || !(scopeDate.DayOfWeek == DayOfWeek.Saturday))
            {
                returnDate = scopeDate;
                break;
            }

            scopeDate = scopeDate.AddDays(1);
        }

        var incomes = IncomeBuilder.Collection(user, 3);
        var debits = DebitBuilder.Collection(user, 3);

        foreach(var income in incomes)
            income.ReceivedOn = date;

        foreach(var debit in debits)
            debit.PaidOn = date;

        var useCase = CreateUseCase(user, date, returnDate, incomes, debits);

        var response = await useCase.Execute(date);

        response.Date.ShouldBe(date);
        response.Incomes.Count.ShouldBe(incomes.Count);
        response.Debits.Count.ShouldBe(debits.Count);
    }

    private static GetCalendarDayUseCase CreateUseCase(
        MobileFinance.Domain.Entities.User user,
        DateTime date,
        DateTime returnDate,
        IList<MobileFinance.Domain.Entities.Income>? incomes = null,
        IList<MobileFinance.Domain.Entities.Debit>? debits = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var recurrenceServiceBuilder = new RecurrenceServiceBuilder();
        var incomeReadOnlyRepositoryBuilder = new IncomeReadOnlyRepositoryBuilder();
        var debitReadOnlyRepositoryBuilder = new DebitReadOnlyRepositoryBuilder();
        var businessDayService = new BusinessDayServiceBuilder().GetNextBusinessDay(date, returnDate).GetNthBusinessDay(date, returnDate).Build();

        if(incomes is not null && debits is not null)
        {
            recurrenceServiceBuilder.GetOcurrences(incomes);
            recurrenceServiceBuilder.GetOcurrences(debits);

            incomeReadOnlyRepositoryBuilder.GetByPeriod(user, incomes);
            debitReadOnlyRepositoryBuilder.GetByPeriod(user, debits);
        }

        return new GetCalendarDayUseCase(
            loggedUser,
            recurrenceServiceBuilder.Build(),
            incomeReadOnlyRepositoryBuilder.Build(),
            debitReadOnlyRepositoryBuilder.Build(),
            businessDayService);
    }
}
