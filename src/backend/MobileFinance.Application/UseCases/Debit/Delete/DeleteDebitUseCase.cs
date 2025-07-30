
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.Debit;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;

namespace MobileFinance.Application.UseCases.Debit.Delete;
public class DeleteDebitUseCase : IDeleteDebitUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IDebitReadOnlyRepository _readOnlyRepository;
    private readonly IDebitWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDebitUseCase(
        ILoggedUser loggedUser,
        IDebitReadOnlyRepository readOnlyRepository,
        IDebitWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long debitId)
    {
        var loggedUser = await _loggedUser.GetUser();

        _ = await _readOnlyRepository.GetById(loggedUser, debitId)
            ?? throw new NotFoundException(ExceptionMessages.DEBIT_NOT_FOUND);

        await _writeOnlyRepository.Delete(debitId);
        await _unitOfWork.Commit();
    }
}
