using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileFinance.Application.Services.AutoMapper;
using Sqids;

namespace MobileFinance.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddIdEncoder(services, configuration);
        AddAutoMapper(services, configuration);
    }

    private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
    {
        var sqids = new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
        });

        services.AddSingleton(sqids);
    }

    private static void AddAutoMapper(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider => new MapperConfiguration(mapperConfiguration =>
        {
            var sqids = provider.GetRequiredService<SqidsEncoder<long>>();
            mapperConfiguration.AddProfile(new MappingProfile(sqids));
            mapperConfiguration.LicenseKey = configuration.GetValue<string>("Settings:AutoMapperLicenseKey")!;
        }, null).CreateMapper());
    }
}
