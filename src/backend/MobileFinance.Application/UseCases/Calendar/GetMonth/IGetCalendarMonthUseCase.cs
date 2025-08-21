using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Calendar.GetMonth;
public interface IGetCalendarMonthUseCase
{
    public Task<ResponseCalendarMonthJson> Execute(int year, int month);
}
