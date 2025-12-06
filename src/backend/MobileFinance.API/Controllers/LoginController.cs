using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using MobileFinance.Application.UseCases.Login.DoLogin;
using MobileFinance.Application.UseCases.Login.External;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using System.Security.Claims;

namespace MobileFinance.API.Controllers;
public class LoginController : MobileFinanceBaseController
{
    /// <summary>
    /// Realiza login de um usuário.
    /// </summary>
    /// <remarks>Endpoint para autenticação de um usuário</remarks>
    /// <param name="request">Objeto com 2 propriedades: E-mail e Senha.</param>
    /// <response status="200">Retorna um objeto contendo um JWT para realizar login.</response>
    /// <response status="401">Retorna um objeto detalhando o erro.</response>
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

    /// <summary>
    /// Realiza login de um usuário com provedor externo (Google).
    /// </summary>
    /// <remarks>
    /// Endpoint para autenticação de um usuário utilizando sua conta do Google.
    /// Este endpoint só pode ser acessado via frontend, pois redireciona o usuário para a página de login do Google.
    /// </remarks>
    /// <param name="returnUrl">Endereço para onde o usuário será redirecionado após realizar login.</param>
    /// <response status="200">Redireciona o usuário para o endereço especificado no parâmetro.</response>
    [HttpGet]
    [Route("google")]
    public async Task<IActionResult> LoginGoogle(
        string returnUrl,
        IExternalLoginUseCase useCase)
    {
        var authenticate = await Request.HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if(IsUserAuthenticated(authenticate))
        {
            var claims = authenticate.Principal!.Identities.First().Claims;

            var name = claims.First(c => c.Type == ClaimTypes.Name).Value;
            var email = claims.First(c => c.Type == ClaimTypes.Email).Value;

            var token = await useCase.Execute(name, email);

            return Redirect($"{returnUrl}/{token}");
        }
        else
            return Challenge(GoogleDefaults.AuthenticationScheme);
    }
}
