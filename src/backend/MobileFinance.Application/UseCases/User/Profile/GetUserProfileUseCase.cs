﻿using AutoMapper;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Services;

namespace MobileFinance.Application.UseCases.User.Profile;
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.GetUser();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
