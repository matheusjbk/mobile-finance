using FluentMigrator;

namespace MobileFinance.Infra.Migrations.Versions;
[Migration(DatabaseVersions.USERS_TABLE, "Create table to save user's information")]
public class Version1 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable();
    }
}
