using FluentMigrator;

namespace MobileFinance.Infra.Migrations.Versions;
[Migration(DatabaseVersions.INCOMES_TABLE, "Create table to save income's informations")]
public class Version3 : VersionBase
{
    public override void Up()
    {
        CreateTable("Incomes")
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Amount").AsInt64().NotNullable()
            .WithColumn("IncomeType").AsInt32().NotNullable()
            .WithColumn("DayOfMonth").AsByte().Nullable()
            .WithColumn("UseBusinessDay").AsBoolean().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Incomes_User_Id", "Users", "Id");
    }
}
