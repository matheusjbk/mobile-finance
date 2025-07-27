using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Income.GetByDayOfMonth;
using Shouldly;

namespace UseCases.Test.Income.GetByDayOfMonth;
public class GetIncomeByDayOfMonthUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var incomes = IncomeBuilder.Collection(user);
        var request = RequestFilterIncomesJsonBuilder.Build();
        var useCase = CreateUseCase(user, incomes);

        var response = await useCase.Execute(request);

        response.Incomes.ShouldNotBeNull();
        response.Incomes.Count.ShouldBe(incomes.Count);
    }

    private static GetIncomeByDayOfMonthUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, IList<MobileFinance.Domain.Entities.Income>? incomes = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryBuilder = new IncomeReadOnlyRepositoryBuilder();

        if(incomes is not null)
            repositoryBuilder.GetByDayOfMonth(user, incomes);

        return new GetIncomeByDayOfMonthUseCase(loggedUser, repositoryBuilder.Build());
    }
}
