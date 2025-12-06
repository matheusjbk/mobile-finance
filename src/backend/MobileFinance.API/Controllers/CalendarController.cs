using Microsoft.AspNetCore.Mvc;
using MobileFinance.API.Attributes;
using MobileFinance.Application.UseCases.Calendar.GetDay;
using MobileFinance.Application.UseCases.Calendar.GetMonth;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;

[AuthenticatedUser]
public class CalendarController : MobileFinanceBaseController
{
    /// <summary>
    /// Mostra entradas e saídas do usuário no mês especificado.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="year">Ano a ser buscado.</param>
    /// <param name="month">Mês a ser buscado.</param>
    /// <response status="200">Retorna um objeto contendo todos os dias do mês e suas entradas e saídas.</response>
    [HttpGet("month")]
    [ProducesResponseType(typeof(ResponseCalendarMonthJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMonth(
        [FromQuery] int year,
        [FromQuery] int month,
        IGetCalendarMonthUseCase useCase)
    {
        var response = await useCase.Execute(year, month);

        return Ok(response);
    }

    /// <summary>
    /// Mostra entradas e saídas do usuário em um dia específico.
    /// </summary>
    /// <remarks>Este endpoint deve ser usado somente caso o usuário tenha feito login.</remarks>
    /// <param name="date">Data a ser buscada. Formato YYYY-MM-DD</param>
    /// <response status="200">Retorna um objeto contendo o dia especificado e suas entradas e saídas.</response>
    [HttpGet("day")]
    [ProducesResponseType(typeof(ResponseCalendarDayJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDay(
        [FromQuery] DateTime date,
        IGetCalendarDayUseCase useCase)
    {
        var response = await useCase.Execute(date);

        return Ok(response);
    }
}
