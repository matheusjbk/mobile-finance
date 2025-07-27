using FluentMigrator;

namespace MobileFinance.Infra.Migrations.Versions;
[Migration(DatabaseVersions.DEBITS_TABLE, "Create table to save debit's informations")]
public class Version4 : VersionBase
{
    public override void Up()
    {
        CreateTable("Debits")
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Amount").AsInt64().NotNullable()
            .WithColumn("DebitType").AsInt32().NotNullable()
            .WithColumn("PaidOn").AsDateTime().NotNullable()
            .WithColumn("UseBusinessDay").AsBoolean().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Debits_User_Id", "Users", "Id");
    }
}
