using Mapster;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories.Income;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Income.GetById;
public class GetIncomeByIdUseCase : IGetIncomeByIdUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IIncomeReadOnlyRepository _repository;

    public GetIncomeByIdUseCase(ILoggedUser loggedUser, IIncomeReadOnlyRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task<ResponseIncomeJson> Execute(long incomeId)
    {
        var loggedUser = await _loggedUser.GetUser();

        var income = await _repository.GetById(loggedUser, incomeId)
            ?? throw new NotFoundException(ExceptionMessages.INCOME_NOT_FOUND);

        return income.Adapt<ResponseIncomeJson>();
    }
}
