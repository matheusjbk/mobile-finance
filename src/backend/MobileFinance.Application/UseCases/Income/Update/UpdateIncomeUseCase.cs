using Mapster;
using MobileFinance.Communication.Requests;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.Income;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Income.Update;
public class UpdateIncomeUseCase : IUpdateIncomeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IIncomeUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateIncomeUseCase(
        ILoggedUser loggedUser,
        IIncomeUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestIncomeJson request, long incomeId)
    {
        Validate(request);

        var loggedUser = await _loggedUser.GetUser();

        var income = await _repository.GetById(loggedUser, incomeId)
            ?? throw new NotFoundException(ExceptionMessages.INCOME_NOT_FOUND);

        request.Adapt(income);

        _repository.Update(income);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestIncomeJson request)
    {
        var validationResult = new IncomeValidator().Validate(request);

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([..validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
