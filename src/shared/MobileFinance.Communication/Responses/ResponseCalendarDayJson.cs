namespace MobileFinance.Communication.Responses;
public class ResponseCalendarDayJson
{
    public DateTime Date { get; set; }
    public IList<ResponseShortIncomeJson> Incomes { get; set; } = [];
    public IList<ResponseShortDebitJson> Debits { get; set; } = [];
}
