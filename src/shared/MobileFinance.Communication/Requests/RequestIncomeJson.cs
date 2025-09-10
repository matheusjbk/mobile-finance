using MobileFinance.Communication.Enums;

namespace MobileFinance.Communication.Requests;
public class RequestIncomeJson
{
    public string Title { get; set; } = string.Empty;
    public long Amount { get; set; }
    public IncomeType IncomeType { get; set; }
    public int? RecurrenceMonthsCount { get; set; }
    public DateTime ReceivedOn { get; set; }
    public bool? UseBusinessDay { get; set; }
}
