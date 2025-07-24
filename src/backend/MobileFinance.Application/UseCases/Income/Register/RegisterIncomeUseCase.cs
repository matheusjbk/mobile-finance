using Mapster;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.Income;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Income.Register;
public class RegisterIncomeUseCase : IRegisterIncomeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IIncomeWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterIncomeUseCase(
        ILoggedUser loggedUser,
        IIncomeWriteOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseIncomeJson> Execute(RequestIncomeJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.GetUser();

        var income = request.Adapt<Domain.Entities.Income>();
        income.UserId = loggedUser.Id;

        await _repository.Add(income);
        await _unitOfWork.Commit();

        return income.Adapt<ResponseIncomeJson>();
    }

    private static void Validate(RequestIncomeJson request)
    {
        var validationResult = new IncomeValidator().Validate(request);

        if(!validationResult.IsValid)
            throw new ErrorOnValidationException([..validationResult.Errors.Select(e => e.ErrorMessage)]);
    }
}
