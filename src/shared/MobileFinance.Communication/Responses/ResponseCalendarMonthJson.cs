namespace MobileFinance.Communication.Responses;
public class ResponseCalendarMonthJson
{
    public int Year { get; set; }
    public int Month { get; set; }
    public IList<ResponseCalendarDayJson> Days { get; set; } = [];
}
