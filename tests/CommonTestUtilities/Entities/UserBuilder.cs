﻿using Bogus;
using CommonTestUtilities.Cryptography;
using MobileFinance.Domain.Entities;

namespace CommonTestUtilities.Entities;
public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var password = new Faker().Internet.Password();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        var user = new Faker<User>()
            .RuleFor(user => user.Id, () => 1)
            .RuleFor(user => user.Name, f => f.Person.FirstName)
            .RuleFor(user => user.Email, (f, user) => f.Internet.Email(user.Name))
            .RuleFor(user => user.Password, () => passwordEncrypter.Encrypt(password))
            .RuleFor(user => user.UserIdentifier, f => Guid.NewGuid());

        return (user, password);
    }
}
