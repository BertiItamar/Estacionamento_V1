using Infrastructure.Entities;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Tests;
using Xunit;

namespace Test.IntegrationTests.Controllers;

public class PriceListControllerTest
{
    [Fact]
    public async Task GetAllPriceListAsync_ReturnsOk()
    {
        var response = await HttpHandler.SendRequest("PriceList", "", "GET");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PriceListGetByIdAsync_ReturnsOk()
    {
        var response = await HttpHandler.SendRequest("PriceList/GetById?id=1", "", "GET");
        var responseContent = await response.Content.ReadAsStringAsync();
        var priceListResponse = JsonConvert.DeserializeObject<PriceList>(responseContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.IsType<PriceList>(priceListResponse);
    }

    [Fact]
    public async Task PriceListGetByIdAsync_ReturnsInternalServerError()
    {
        var response = await HttpHandler.SendRequest("PriceList/GetById?id=1000", "", "GET");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task PriceListPostAsync_ReturnsOk()
    {
        // Arrange
        var priceList = new PriceList
        {
            InitialTimeValue = 5,
            AdditionalHourlyValue = 1,
            InitialDate = new DateTime(2023, 1, 1),
            FinalDate = new DateTime(2023, 1, 12),
            IsActive = true
        };

        // Act
        var response = await HttpHandler.SendRequest("PriceList", priceList, "POST");
        var responseContent = await response.Content.ReadAsStringAsync();
        var priceListResponse = JsonConvert.DeserializeObject<PriceList>(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, priceListResponse.AdditionalHourlyValue);
        Assert.IsType<PriceList>(priceListResponse);
    }

    [Fact]
    public async Task PriceListPutAsync_ReturnsOk()
    {
        // Arrange
        var priceList = new PriceList
        {
            Id = 1,
            InitialTimeValue = 5,
            AdditionalHourlyValue = 2,
            InitialDate = new DateTime(2023, 1, 1),
            FinalDate = new DateTime(2023, 1, 12),
            IsActive = true
        };

        // Act
        var response = await HttpHandler.SendRequest("PriceList", priceList, "PUT");
        var responseContent = await response.Content.ReadAsStringAsync();
        var priceListResponse = JsonConvert.DeserializeObject<PriceList>(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, priceListResponse.AdditionalHourlyValue);
        Assert.IsType<PriceList>(priceListResponse);
    }

    [Fact]
    public async Task PriceListPutAsync__ReturnsInternalServerError()
    {
        // Arrange
        var priceList = new PriceList();

        // Serialize the empty object as JSON
        var emptyBody = new StringContent(JsonConvert.SerializeObject(priceList), Encoding.UTF8, "application/json");

        // Act
        var response = await HttpHandler.SendRequest("PriceList", emptyBody, "PUT");

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task PriceListDeleteAsync_ReturnsOk()
    {
        var response = await HttpHandler.SendRequest("PriceList?id=2", "", "DELETE");
        var responseContent = await response.Content.ReadAsStringAsync();
        var PriceListResponse = JsonConvert.DeserializeObject<PriceList>(responseContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.IsType<PriceList>(PriceListResponse);
    }
    [Fact]
    public async Task PriceListDeleteAsync_ReturnsInternalServer()
    {
        var response = await HttpHandler.SendRequest("PriceList?id=1000", "", "DELETE");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}
