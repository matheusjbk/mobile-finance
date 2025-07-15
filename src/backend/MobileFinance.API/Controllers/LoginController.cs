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
