using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Communication.Requests;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Debit.GetByDayOfMonth;
public class GetDebitByDayOfMonthTest : MobileFinanceClassFixture
{
    private const string ROUTE = "debit/filter-by-day";
    private readonly Guid _userIdentifier;
    private readonly DateTime _debitPaidDate;

    public GetDebitByDayOfMonthTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _debitPaidDate = factory.GetDebitPaidDate();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = new RequestFilterDebitsJson { FilterDate =  _debitPaidDate };

        var response = await DoPost(ROUTE, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("debits").EnumerateArray().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestFilterDebitsJsonBuilder.Build();

        var response = await DoPost(ROUTE, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
