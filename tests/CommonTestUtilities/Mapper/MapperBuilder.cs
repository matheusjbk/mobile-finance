using AutoMapper;
using CommonTestUtilities.Cryptography;
using Microsoft.Extensions.Logging.Abstractions;
using MobileFinance.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;
public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncoder = IdEncoderBuilder.Build();

        return new MapperConfiguration(mapperConfiguration =>
        {
            mapperConfiguration.AddProfile(new MappingProfile(idEncoder));
        }, new NullLoggerFactory()).CreateMapper();
    }
}
