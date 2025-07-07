using Microsoft.Extensions.Configuration;

namespace MobileFinance.Infra.Extensions;
public static class ConfigurationExtensions
{
    public static string ConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("Connection_MySql")!;

    public static bool IsTestEnvironment(this IConfiguration configuration) => configuration.GetValue<bool>("InMemoryTest");
}
