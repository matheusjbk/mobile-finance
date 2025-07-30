using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Tokens;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Debit.GetById;
public class GetDebitByIdTest : MobileFinanceClassFixture
{
    private const string ROUTE = "debit";
    private readonly Guid _userIdentifier;
    private readonly string _debitId;

    public GetDebitByIdTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _debitId = factory.GetDebitId();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(route: $"{ROUTE}/{_debitId}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("id").GetString().ShouldBe(_debitId);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Debit_NotFound(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var id = IdEncoderBuilder.Build().Encode(1000);

        var response = await DoGet(route: $"{ROUTE}/{id}", token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("DEBIT_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }

}
