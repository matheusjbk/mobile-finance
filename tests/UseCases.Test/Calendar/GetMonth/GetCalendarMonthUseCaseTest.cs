using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.RecurrenceService;
using CommonTestUtilities.Repositories;
using MobileFinance.Application.UseCases.Calendar.GetMonth;
using Shouldly;

namespace UseCases.Test.Calendar.GetMonth;
public class GetCalendarMonthUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var year = DateTime.UtcNow.Year;
        var month = DateTime.UtcNow.Month;
        var incomes = IncomeBuilder.Collection(user, 3);
        var debits = DebitBuilder.Collection(user, 3);
        var useCase = CreateUseCase(user, incomes, debits);

        var response = await useCase.Execute(year, month);

        response.ShouldNotBeNull();
        response.Year.ShouldBe(year);
        response.Month.ShouldBe(month);
        response.Days.ShouldNotBeEmpty();
    }

    private static GetCalendarMonthUseCase CreateUseCase(
        MobileFinance.Domain.Entities.User user,
        IList<MobileFinance.Domain.Entities.Income>? incomes = null,
        IList<MobileFinance.Domain.Entities.Debit>? debits = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var recurrenceServiceBuilder = new RecurrenceServiceBuilder();
        var incomeReadOnlyRepositoryBuilder = new IncomeReadOnlyRepositoryBuilder();
        var debitReadOnlyRepositoryBuilder = new DebitReadOnlyRepositoryBuilder();

        if(incomes is not null && debits is not null)
        {
            recurrenceServiceBuilder.GetOcurrences(incomes);
            recurrenceServiceBuilder.GetOcurrences(debits);

            incomeReadOnlyRepositoryBuilder.GetByPeriod(user, incomes);
            debitReadOnlyRepositoryBuilder.GetByPeriod(user, debits);
        }

        return new GetCalendarMonthUseCase(
            loggedUser,
            recurrenceServiceBuilder.Build(),
            incomeReadOnlyRepositoryBuilder.Build(),
            debitReadOnlyRepositoryBuilder.Build());
    }
}
