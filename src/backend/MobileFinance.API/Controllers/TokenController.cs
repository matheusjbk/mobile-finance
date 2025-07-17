using Microsoft.AspNetCore.Mvc;
using MobileFinance.Application.UseCases.Token.RefreshToken;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;

public class TokenController : MobileFinanceBaseController
{
    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RefreshToken(
        IUseRefreshTokenUseCase useCase,
        RequestNewTokenJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
