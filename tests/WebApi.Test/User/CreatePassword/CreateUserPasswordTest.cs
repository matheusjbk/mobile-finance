using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Communication.Requests;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.CreatePassword;
public class CreateUserPasswordTest : MobileFinanceClassFixture
{
    private const string ROUTE = "user/create-password";
    private readonly string _userEmail;
    private readonly Guid _userIdentifier;

    public CreateUserPasswordTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userEmail = factory.GetUserEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestCreateUserPasswordJsonBuilder.Build();
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(route: ROUTE, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _userEmail,
            Password = request.Password,
        };

        response = await DoPost(route: "login", request: loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Password(string culture)
    {
        var request = new RequestCreateUserPasswordJson { Password = string.Empty };
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(route: ROUTE, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("EMPTY_PASSWORD", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
