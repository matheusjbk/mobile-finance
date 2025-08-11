namespace MobileFinance.Domain.Entities;
public class CalendarDay : EntityBase
{
    public DateTime Date { get; set; }
    public bool IsHoliday { get; set; } = false;
    public bool IsWeekend { get; set; } = false;
    public bool IsBusinessDay { get; set; } = true;
}
