using FluentMigrator;

namespace MobileFinance.Infra.Migrations.Versions;
[Migration(DatabaseVersions.CALENDAR_DAYS_TABLE, "Create table to save calendar days w/ weekends and holidays infos")]
public class Version5 : VersionBase
{
    public override void Up()
    {
        CreateTable("CalendarDays")
            .WithColumn("Date").AsDateTime().NotNullable()
            .WithColumn("IsHoliday").AsBoolean().NotNullable()
            .WithColumn("IsWeekend").AsBoolean().NotNullable()
            .WithColumn("IsBusinessDay").AsBoolean().NotNullable();
    }
}
