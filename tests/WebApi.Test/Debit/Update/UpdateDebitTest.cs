using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Debit.Update;
public class UpdateDebitTest : MobileFinanceClassFixture
{
    private const string ROUTE = "debit";
    private readonly Guid _userIdentifier;
    private readonly string _debitId;

    public UpdateDebitTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _debitId = factory.GetDebitId();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestDebitJsonBuilder.Build();

        var response = await DoPut(route: $"{ROUTE}/{_debitId}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Debit_NotFound(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestDebitJsonBuilder.Build();
        var id = IdEncoderBuilder.Build().Encode(1000);

        var response = await DoPut(route: $"{ROUTE}/{id}", request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("DEBIT_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestDebitJsonBuilder.Build();
        request.Title = string.Empty;

        var response = await DoPut(route: $"{ROUTE}/{_debitId}", request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("EMPTY_DEBIT_TITLE", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
