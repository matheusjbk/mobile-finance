using Microsoft.AspNetCore.Mvc;
using MobileFinance.API.Attributes;
using MobileFinance.API.Binders;
using MobileFinance.Application.UseCases.Income.Delete;
using MobileFinance.Application.UseCases.Income.GetById;
using MobileFinance.Application.UseCases.Income.Register;
using MobileFinance.Application.UseCases.Income.Update;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;

[AuthenticatedUser]
public class IncomeController : MobileFinanceBaseController
{
    /// <summary>
    /// Registra uma entrada e a associa a um usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="request">
    /// Objeto com 7 propriedades:
    /// Title: Título da entrada.
    /// Amount: Valor da entrada.
    /// IncomeType: Tipo da entrada (0 - única, 1 - salário, 2 - recorrente).
    /// RecurrenceMonthsCount: Quantidade de meses que a entrada irá se repetir (obrigatório se IncomeType for 2).
    /// ReceivedOn: Data em que a entrada foi ou será recebida (formato: AAAA-MM-DD. obrigatório se IncomeType for 0 ou 2).
    /// BusinessDayNumber: Número do dia útil do mês em que a entrada será recebida (obrigatório se IncomeType for 1).
    /// UseBusinessDay: Indica se deve usar o dia útil para calcular a data de recebimento (obrigatório se IncomeType for 1 ou 2).
    /// </param>
    /// <response status="201">Retorna um objeto contendo o ID, título e quantia da entrada.</response>
    /// <response status="400">Retorna um objeto contendo detalhes do erro.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredIncomeJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        IRegisterIncomeUseCase useCase,
        RequestIncomeJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    /// <summary>
    /// Mostra uma entrada específica do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="id">ID da entrada.</param>
    /// <response status="200">Retorna um objeto contendo todas as informações da entrada.</response>
    /// <response status="404">Retorna um objeto contendo detalhes do erro.</response>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseIncomeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        IGetIncomeByIdUseCase useCase,
        [FromRoute] [ModelBinder(typeof(MobileFinanceIdBinder))] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

    /// <summary>
    /// Altera uma entrada especifica do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="request">
    /// Objeto com 7 propriedades:
    /// Title: Título da entrada.
    /// Amount: Valor da entrada.
    /// IncomeType: Tipo da entrada (0 - única, 1 - salário, 2 - recorrente).
    /// RecurrenceMonthsCount: Quantidade de meses que a entrada irá se repetir (obrigatório se IncomeType for 2).
    /// ReceivedOn: Data em que a entrada foi ou será recebida (formato: AAAA-MM-DD. obrigatório se IncomeType for 0 ou 2).
    /// BusinessDayNumber: Número do dia útil do mês em que a entrada será recebida (obrigatório se IncomeType for 1).
    /// UseBusinessDay: Indica se deve usar o dia útil para calcular a data de recebimento (obrigatório se IncomeType for 1 ou 2).
    /// </param>
    /// <param name="id">ID da entrada.</param>
    /// <response status="404">Retorna um objeto contendo detalhes do erro.</response>
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        IUpdateIncomeUseCase useCase,
        RequestIncomeJson request,
        [FromRoute][ModelBinder(typeof(MobileFinanceIdBinder))] long id)
    {
        await useCase.Execute(request, id);

        return NoContent();
    }

    /// <summary>
    /// Deleta uma entrada específica do usuário.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="id">ID da entrada.</param>
    /// <response status="404">Retorna um objeto contendo detalhes do erro.</response>
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        IDeleteIncomeUseCase useCase,
        [FromRoute][ModelBinder(typeof(MobileFinanceIdBinder))] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
