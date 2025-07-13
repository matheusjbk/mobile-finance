using AutoMapper;
using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Entities;
using Sqids;

namespace MobileFinance.Application.Services.AutoMapper;
public class MappingProfile : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;

    public MappingProfile(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, options => options.Ignore());
    }

    private void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
    }
}
