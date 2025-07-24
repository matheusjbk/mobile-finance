using MobileFinance.Domain.Enums;

namespace MobileFinance.Domain.Entities;
public class Income : EntityBase
{
    public long Amount { get; set; }
    public IncomeType IncomeType { get; set; }
    public byte? DayOfMonth { get; set; }
    public bool UseBusinessDay { get; set; } = false;
    public long UserId { get; set; }
}
