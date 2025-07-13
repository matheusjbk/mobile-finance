using MobileFinance.Communication.Requests;

namespace MobileFinance.Application.UseCases.User.Update;
public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}
