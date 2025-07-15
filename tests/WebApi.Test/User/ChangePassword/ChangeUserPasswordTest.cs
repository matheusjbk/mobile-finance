using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Communication.Requests;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.ChangePassword;
public class ChangeUserPasswordTest : MobileFinanceClassFixture
{
    private const string ROUTE = "user/change-password";
    private readonly string _userEmail;
    private readonly string _userPassword;
    private readonly Guid _userIdentifier;

    public ChangeUserPasswordTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userEmail = factory.GetUserEmail();
        _userPassword = factory.GetUserPassword();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = _userPassword;

        var response = await DoPut(route: ROUTE, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _userEmail,
            Password = _userPassword
        };

        response = await DoPost(route: "login", request: loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost(route: "login", request: loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Different_Current_Password(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestChangePasswordJsonBuilder.Build();

        var response = await DoPut(route: ROUTE, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
