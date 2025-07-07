namespace MobileFinance.Domain.Entities;
public class EntityBase
{
    public long Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
