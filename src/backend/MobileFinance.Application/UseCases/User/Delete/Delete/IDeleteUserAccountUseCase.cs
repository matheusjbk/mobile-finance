﻿namespace MobileFinance.Application.UseCases.User.Delete.Delete;
public interface IDeleteUserAccountUseCase
{
    public Task Execute(Guid userIdentifier);
}
