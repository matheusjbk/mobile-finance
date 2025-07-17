using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using MobileFinance.Application.UseCases.User.Delete.Delete;
using Shouldly;

namespace UseCases.Test.User.Delete.Delete;
public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(user.UserIdentifier);

        await act.ShouldNotThrowAsync();
    }

    private DeleteUserAccountUseCase CreateUseCase()
    {
        var repository = UserDeleteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteUserAccountUseCase(repository, unitOfWork);
    }
}
