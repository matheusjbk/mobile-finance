using Microsoft.AspNetCore.Mvc;
using MobileFinance.Application.UseCases.Login.DoLogin;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;
public class LoginController : MobileFinanceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        IDoLoginUseCase useCase,
        RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
