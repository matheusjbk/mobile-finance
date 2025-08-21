using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Calendar.GetDay;
public interface IGetCalendarDayUseCase
{
    public Task<ResponseCalendarDayJson> Execute(DateTime date);
}
