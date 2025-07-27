using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Communication.Requests;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Income.GetByDayOfMonth;
public class GetIncomeByDayOfMonthTest : MobileFinanceClassFixture
{
    private const string ROUTE = "income/filter-by-day";
    private readonly Guid _userIdentifier;
    private readonly DateTime _incomeReceivedDate;

    public GetIncomeByDayOfMonthTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _incomeReceivedDate = factory.GetIncomeReveicedDate();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = new RequestFilterIncomesJson { FilterDate =  _incomeReceivedDate };

        var response = await DoPost(ROUTE, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("incomes").EnumerateArray().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestFilterIncomesJsonBuilder.Build();

        var response = await DoPost(ROUTE, request, token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}
