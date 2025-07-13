﻿using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, (f, request) => f.Internet.Email(request.Name));
    }
}
