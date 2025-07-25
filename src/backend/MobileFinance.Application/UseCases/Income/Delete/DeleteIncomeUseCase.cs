
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.Income;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Income.Delete;
public class DeleteIncomeUseCase : IDeleteIncomeUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IIncomeReadOnlyRepository _readOnlyRepository;
    private readonly IIncomeWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteIncomeUseCase(
        ILoggedUser loggedUser,
        IIncomeReadOnlyRepository readOnlyRepository,
        IIncomeWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long incomeId)
    {
        var loggedUser = await _loggedUser.GetUser();

        _ = await _readOnlyRepository.GetById(loggedUser, incomeId)
            ?? throw new NotFoundException(ExceptionMessages.INCOME_NOT_FOUND);

        await _writeOnlyRepository.Delete(incomeId);
        await _unitOfWork.Commit();
    }
}
