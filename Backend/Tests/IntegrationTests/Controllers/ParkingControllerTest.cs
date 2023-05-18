using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Tests.IntegrationTests.Controllers
{
    public class ParkingControllerTest
    {
        [Fact]
        public async Task GetAllParkingAsync_ReturnsOk()
        {
            // Arrange
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:7021/api/Parking");

            // Act
            var response = await httpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
