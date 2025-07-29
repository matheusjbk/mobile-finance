using Microsoft.AspNetCore.Mvc;
using MobileFinance.API.Attributes;
using MobileFinance.API.Binders;
using MobileFinance.Application.UseCases.Debit.GetById;
using MobileFinance.Application.UseCases.Debit.Register;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;

[AuthenticatedUser]
public class DebitController : MobileFinanceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredDebitJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        IRegisterDebitUseCase useCase,
        RequestDebitJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseDebitJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        IGetDebitByIdUseCase useCase,
        [FromRoute][ModelBinder(typeof(MobileFinanceIdBinder))] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }
}
