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

    [HttpGet]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [AuthenticatedUser]
    public async Task<IActionResult> Profile(IGetUserProfileUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }

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

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AuthenticatedUser]
    public async Task<IActionResult> Delete(IRequestDeleteUserUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}
