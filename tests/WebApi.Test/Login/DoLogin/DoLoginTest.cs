using CommonTestUtilities.Requests;
using MobileFinance.Communication.Requests;
using MobileFinance.Exceptions;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : MobileFinanceClassFixture
{
    private const string ROUTE = "login";
    private readonly string _userName;
    private readonly string _userEmail;
    private readonly string _userPassword;

    public DoLoginTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userName = factory.GetUserName();
        _userEmail = factory.GetUserEmail();
        _userPassword = factory.GetUserPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _userEmail,
            Password = _userPassword
        };

        var response = await DoPost(ROUTE, request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.ShouldSatisfyAllConditions(
            () => responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace(),
            () => responseData.RootElement.GetProperty("name").GetString().ShouldBe(_userName),
            () => responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty(),
            () => responseData.RootElement.GetProperty("tokens").GetProperty("refreshToken").GetString().ShouldNotBeNullOrEmpty());
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();

        var response = await DoPost(ROUTE, request);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("INVALID_EMAIL_OR_PASSWORD");

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
