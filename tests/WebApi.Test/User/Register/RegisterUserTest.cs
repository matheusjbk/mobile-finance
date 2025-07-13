using CommonTestUtilities.Requests;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Register;
public class RegisterUserTest : MobileFinanceClassFixture
{
    private const string ROUTE = "user";

    public RegisterUserTest(MobileFinanceWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var response = await DoPost(route: ROUTE, request: request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.ShouldSatisfyAllConditions(
            () => responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace(),
            () => responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name),
            () => responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty(),
            () => responseData.RootElement.GetProperty("tokens").GetProperty("refreshToken").GetString().ShouldNotBeNullOrEmpty());
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await DoPost(route: ROUTE, request: request, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("EMPTY_NAME", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
