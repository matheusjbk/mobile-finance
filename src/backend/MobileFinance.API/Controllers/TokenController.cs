using Microsoft.AspNetCore.Mvc;
using MobileFinance.Application.UseCases.Token.RefreshToken;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;

public class TokenController : MobileFinanceBaseController
{
    /// <summary>
    /// Mostra entradas e saídas do usuário no mês especificado.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="request">Refresh token do usuário</param>
    /// <response status="200">Retorna um objeto contendo um novo token e um novo refresh token.</response>
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
