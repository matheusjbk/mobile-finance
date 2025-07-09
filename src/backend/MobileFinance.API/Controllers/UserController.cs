using Microsoft.AspNetCore.Mvc;
using MobileFinance.Application.UseCases.User.Register;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;
public class UserController : MobileFinanceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        IRegisterUserUseCase useCase,
        RequestRegisterUserJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
