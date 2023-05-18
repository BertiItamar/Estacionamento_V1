using DesafioEstacionamentoBenner.DTO_s;
using Infrastructure.Entities;
using Newtonsoft.Json;
using System.Net;
using Xunit;

namespace Tests.IntegrationTests.Controllers;

public class ParkingControllerTest
{
    [Fact]
    public async Task GetAllParkingAsync_ReturnsOk()
    {
        var response = await HttpHandler.SendRequest("Parking", "", "GET");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ParkingGetByIdAsync_ReturnsOk()
    {
        var response = await HttpHandler.SendRequest("Parking?id=1", "", "GET");
        var responseContent = await response.Content.ReadAsStringAsync();
        var parkingResponse = JsonConvert.DeserializeObject<Parking>(responseContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.IsType<Parking>(parkingResponse);
    }

    [Fact]
    public async Task ParkingGetByIdAsync_ReturnsNotFound()
    {
        var response = await HttpHandler.SendRequest("Parking?id=1000", "", "GET");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task ParkingPostAsync_ReturnsBadRequest()
    {
        // Arrange
        var parking = new Parking
        {
            LicensePlate = "TESTE",
            Model = "Corsa",
            EntryDate = DateTime.Now
        };

        // Act
        var response = await HttpHandler.SendRequest("Parking/RegisterEntry", parking, "POST");
        var responseContent = await response.Content.ReadAsStringAsync();
        var parkingResponse = JsonConvert.DeserializeObject<Parking>(responseContent);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.IsType<Parking>(parkingResponse);
    }

    [Fact]
    public async Task ParkingPutAsync_ReturnsBadRequest()
    {
        // Arrange
        var invalidParking = new RegisterDepartureRequestDTO
        {
            DepartureDate = DateTime.Now.AddDays(1)
        };

        // Act
        var response = await HttpHandler.SendRequest("Parking/RegisterDeparture", invalidParking, "PUT");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ParkingtDeleteAsync_ReturnsOk()
    {
        var response = await HttpHandler.SendRequest("Parking?id=2", "", "DELETE");
        var responseContent = await response.Content.ReadAsStringAsync();
        var ParkingtResponse = JsonConvert.DeserializeObject<Parking>(responseContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.IsType<Parking>(ParkingtResponse);
    }

    [Fact]
    public async Task ParkingDeleteAsync_ReturnsInternalServer()
    {
        var response = await HttpHandler.SendRequest("Parking?id=1000", "", "DELETE");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
}
