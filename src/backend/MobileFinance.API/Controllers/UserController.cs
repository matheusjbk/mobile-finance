using Microsoft.AspNetCore.Mvc;
using MobileFinance.API.Attributes;
using MobileFinance.Application.UseCases.User.ChangePassword;
using MobileFinance.Application.UseCases.User.CreatePassword;
using MobileFinance.Application.UseCases.User.Delete.Request;
using MobileFinance.Application.UseCases.User.Profile;
using MobileFinance.Application.UseCases.User.Register;
using MobileFinance.Application.UseCases.User.Update;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;
public class UserController : MobileFinanceBaseController
{
    /// <summary>
    /// Registra um usuário.
    /// </summary>
    /// <remarks>Endpoint para registrar um usuário na base de dados e possibilitar o registro de entradas e saídas.</remarks>
    /// <param name="request">Objeto com 3 propriedades: Nome, E-mail e Senha.</param>
    /// <response status="201">Retorna um objeto contendo um JWT para realizar login.</response>
    /// <response status="400">Retorna um objeto detalhando o erro.</response>
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

    /// <summary>
    /// Mostra o perfil do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <response status="200">Retorna um objeto contendo nome e e-mail do usuário.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> Profile(IGetUserProfileUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

    /// <summary>
    /// Altera o perfil do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// /// <param name="request">Objeto com 2 propriedades: Nome e E-mail.</param>
    /// <response status="400">Retorna um objeto detalhando o erro.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> Update(
        IUpdateUserUseCase useCase,
        RequestUpdateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    /// <summary>
    /// Altera a senha do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// /// <param name="request">Objeto com 2 propriedades: Senha atual e nova Senha.</param>
    /// <response status="400">Retorna um objeto detalhando o erro.</response>
    [HttpPut]
    [Route("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> ChangePassword(
        IChangeUserPasswordUseCase useCase,
        RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    /// <summary>
    /// Cria uma senha para o usuário.
    /// </summary>
    /// <remarks>
    /// Este endpoint deve ser usado somente caso o usuário tenha feito login.
    /// Endpoint para usuários que se registraram utilizando provedores externos (Google).
    /// </remarks>
    /// /// <param name="request">Objeto com 1 propriedade: Senha.</param>
    /// <response status="400">Retorna um objeto detalhando o erro.</response>
    [HttpPost]
    [Route("create-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [AuthenticatedUser]
    public async Task<IActionResult> CreatePassword(
        RequestCreateUserPasswordJson request,
        ICreateUserPasswordUseCase useCase)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    /// <summary>
    /// Deleta o perfil do usuário.
    /// </summary>
    /// <remarks>
    /// Este endpoint deve ser usado somente caso o usuário tenha feito login.
    /// </remarks>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AuthenticatedUser]
    public async Task<IActionResult> Delete(IRequestDeleteUserUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}
