using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using MobileFinance.Application.UseCases.Debit.GetById;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Debit.GetById;
public class GetDebitDyIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var debit = DebitBuilder.Build(user);
        var useCase = CreateUseCase(user, debit);

        var response = await useCase.Execute(debit.Id);

        var idEncoder = IdEncoderBuilder.Build();

        response.ShouldNotBeNull();
        response.Title.ShouldBe(debit.Title);
    }

    [Fact]
    public async Task Error_Debit_NotFound()
    {
        (var user, _) = UserBuilder.Build();
        var debit = DebitBuilder.Build(user);
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(debit.Id);
        
        var exception = await act.ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.DEBIT_NOT_FOUND);
    }

    private static GetDebitByIdUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, MobileFinance.Domain.Entities.Debit? debit = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryBuilder = new DebitReadOnlyRepositoryBuilder();

        if(debit is not null)
            repositoryBuilder.GetById(user, debit);

        return new GetDebitByIdUseCase(loggedUser, repositoryBuilder.Build());
    }
}
