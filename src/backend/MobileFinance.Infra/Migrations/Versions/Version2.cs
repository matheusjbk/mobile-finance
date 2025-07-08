using FluentMigrator;

namespace MobileFinance.Infra.Migrations.Versions;
[Migration(DatabaseVersions.REFRESH_TOKEN_TABLE, "Create table to save refresh token informations")]
public class Version2 : VersionBase
{
    public override void Up()
    {
        CreateTable("RefreshTokens")
            .WithColumn("Value").AsString().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshTokens_User_Id", "Users", "Id");
    }
}
