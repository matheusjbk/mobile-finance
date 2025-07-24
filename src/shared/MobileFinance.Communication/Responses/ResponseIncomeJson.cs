using MobileFinance.Communication.Enums;

namespace MobileFinance.Communication.Responses;
public class ResponseIncomeJson
{
    public string Id { get; set; } = string.Empty;
    public string Title {  get; set; } = string.Empty;
    public long Amount { get; set; }
    public IncomeType IncomeType { get; set; }
    public byte? DayOfMonth { get; set; }
    public bool UseBusinessDay { get; set; }
}
