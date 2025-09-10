using MobileFinance.Domain.Enums;

namespace MobileFinance.Domain.Entities;
public class Debit : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public long Amount { get; set; }
    public DebitType DebitType { get; set; }
    public int? RecurrenceMonthsCount { get; set; }
    public DateTime PaidOn { get; set; }
    public bool UseBusinessDay { get; set; } = false;
    public long UserId { get; set; }
}
