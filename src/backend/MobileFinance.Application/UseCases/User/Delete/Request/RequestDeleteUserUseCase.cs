
using MobileFinance.Domain.Repositories;
using MobileFinance.Domain.Repositories.User;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Domain.Services.ServiceBus;

namespace MobileFinance.Application.UseCases.User.Delete.Request;
public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeleteUserQueue _queue;

    public RequestDeleteUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork,
        IDeleteUserQueue queue)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _queue = queue;
    }

    public async Task Execute()
    {
        // Entity rescued w/ AsNoTracking(), so, is not recommended to update through it.
        var loggedUser = await _loggedUser.GetUser();

        // Entity to update. Rescued w/o AsNoTracking().
        var user = await _repository.GetById(loggedUser.Id);

        user.Active = false;

        _repository.Update(user);
        await _unitOfWork.Commit();

        await _queue.SendMessage(user);
    }
}
