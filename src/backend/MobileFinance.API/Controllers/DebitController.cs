using Microsoft.AspNetCore.Mvc;
using MobileFinance.API.Attributes;
using MobileFinance.API.Binders;
using MobileFinance.Application.UseCases.Debit.Delete;
using MobileFinance.Application.UseCases.Debit.GetById;
using MobileFinance.Application.UseCases.Debit.Register;
using MobileFinance.Application.UseCases.Debit.Update;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;

[AuthenticatedUser]
public class DebitController : MobileFinanceBaseController
{
    /// <summary>
    /// Registra uma saída e a associa a um usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="request">
    /// Objeto com 7 propriedades:
    /// Title: Título da saída.
    /// Amount: Valor da saída.
    /// DebitType: Tipo da saída (0 - única, 1 - recorrente).
    /// RecurrenceMonthsCount: Quantidade de meses que a saída irá se repetir (obrigatório se DebitType for 1).
    /// PaidOn: Data em que a saída foi ou será paga (formato: AAAA-MM-DD).
    /// UseBusinessDay: Indica se deve usar o dia útil para calcular a data de pagamento (obrigatório se DebitType for 1).
    /// </param>
    /// <response status="201">Retorna um objeto contendo o ID, título e quantia da saída.</response>
    /// <response status="400">Retorna um objeto contendo detalhes do erro.</response>
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

    /// <summary>
    /// Mostra uma saída específica do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="id">ID da saída.</param>
    /// <response status="200">Retorna um objeto contendo todas as informações da entrada.</response>
    /// <response status="404">Retorna um objeto contendo detalhes do erro.</response>
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

    /// <summary>
    /// Altera uma saída especifica do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="request">
    /// Objeto com 7 propriedades:
    /// Title: Título da saída.
    /// Amount: Valor da saída.
    /// DebitType: Tipo da saída (0 - única, 1 - recorrente).
    /// RecurrenceMonthsCount: Quantidade de meses que a saída irá se repetir (obrigatório se DebitType for 1).
    /// PaidOn: Data em que a saída foi ou será paga (formato: AAAA-MM-DD).
    /// UseBusinessDay: Indica se deve usar o dia útil para calcular a data de pagamento (obrigatório se DebitType for 1).
    /// </param>
    /// <param name="id">ID da saída.</param>
    /// <response status="404">Retorna um objeto contendo detalhes do erro.</response>
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        IUpdateDebitUseCase useCase,
        RequestDebitJson request,
        [FromRoute][ModelBinder(typeof(MobileFinanceIdBinder))] long id)
    {
        await useCase.Execute(request, id);

        return NoContent();
    }

    /// <summary>
    /// Deleta uma saída específica do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="id">ID da saída.</param>
    /// <response status="404">Retorna um objeto contendo detalhes do erro.</response>
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        IDeleteDebitUseCase useCase,
        [FromRoute][ModelBinder(typeof(MobileFinanceIdBinder))] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
