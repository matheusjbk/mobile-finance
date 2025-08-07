using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Debit.GetByDayOfMonth;
using Shouldly;

namespace UseCases.Test.Debit.GetByDayOfMonth;
public class GetDebitByDayOfMonthUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var debits = DebitBuilder.Collection(user);
        var request = RequestFilterDebitsJsonBuilder.Build();
        var useCase = CreateUseCase(user, debits);

        var response = await useCase.Execute(request);

        response.Debits.ShouldNotBeNull();
        response.Debits.Count.ShouldBe(debits.Count);
    }

    private static GetDebitByDayOfMonthUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, IList<MobileFinance.Domain.Entities.Debit>? debits = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryBuilder = new DebitReadOnlyRepositoryBuilder();

        if(debits is not null)
            repositoryBuilder.GetByDayOfMonth(user, debits);

        return new GetDebitByDayOfMonthUseCase(loggedUser, repositoryBuilder.Build());
    }
}
