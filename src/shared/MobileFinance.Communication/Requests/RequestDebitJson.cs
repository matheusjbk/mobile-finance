using MobileFinance.Communication.Enums;

namespace MobileFinance.Communication.Requests;
public class RequestDebitJson
{
    public string Title { get; set; } = string.Empty;
    public long Amount { get; set; }
    public DebitType DebitType { get; set; }
    public DateTime PaidOn { get; set; }
    public bool? UseBusinessDay { get; set; }
}
