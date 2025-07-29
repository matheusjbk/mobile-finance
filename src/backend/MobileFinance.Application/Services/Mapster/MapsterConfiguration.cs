using Mapster;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Entities;
using Sqids;

namespace MobileFinance.Application.Services.Mapster;
public class MapsterConfiguration
{
    private readonly SqidsEncoder<long> _idEncoder;

    public MapsterConfiguration(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;
        RequestToDomain();
        DomainToResponse();
    }

    private static void RequestToDomain()
    {
        TypeAdapterConfig<RequestRegisterUserJson, User>
            .NewConfig()
            .Ignore(dest => dest.Password);

        TypeAdapterConfig<RequestIncomeJson, Income>
            .NewConfig();

        TypeAdapterConfig<RequestDebitJson, Debit>
            .NewConfig();
    }

    private void DomainToResponse()
    {
        TypeAdapterConfig<User, ResponseUserProfileJson>
            .NewConfig();

        TypeAdapterConfig<Income, ResponseRegisteredIncomeJson>
            .NewConfig()
            .Map(dest => dest.Id, src => _idEncoder.Encode(src.Id));

        TypeAdapterConfig<Income, ResponseIncomeJson>
            .NewConfig()
            .Map(dest => dest.Id, src => _idEncoder.Encode(src.Id));

        TypeAdapterConfig<Income, ResponseShortIncomeJson>
            .NewConfig()
            .Map(dest => dest.Id, src => _idEncoder.Encode(src.Id));

        TypeAdapterConfig<Debit, ResponseRegisteredDebitJson>
            .NewConfig()
            .Map(dest => dest.Id, src => _idEncoder.Encode(src.Id));

        TypeAdapterConfig<Debit, ResponseDebitJson>
            .NewConfig()
            .Map(dest => dest.Id, src => _idEncoder.Encode(src.Id));
    }
}
