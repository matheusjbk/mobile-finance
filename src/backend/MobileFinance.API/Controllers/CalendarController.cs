using Microsoft.AspNetCore.Mvc;
using MobileFinance.API.Attributes;
using MobileFinance.Application.UseCases.Calendar.GetDay;
using MobileFinance.Application.UseCases.Calendar.GetMonth;
using MobileFinance.Communication.Responses;

namespace MobileFinance.API.Controllers;

[AuthenticatedUser]
public class CalendarController : MobileFinanceBaseController
{
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
