using LondonRoadsStatus;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;
using System.Net.Http;
using System.Threading.Tasks;

namespace LondonRoadsServiceTests
{
    public class LondonRoadsServiceTests
    {
        // If a valid ID is provided, the ID should be returned.
        [Fact]
        public async Task GetRoadStatus_ValidRoadId_ReturnsExpectedResult()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"displayName\":\"A2\"}]"),
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new RoadsStatusService("mockAPIKey", httpClient);

            var result = await service.GetRoadStatus("A2", "", "");

            Assert.Contains("A2", result);
        }

        // If a valid ID and date range are provided, the ID and status severity & description should be returned.
        [Fact]
        public async Task GetRoadStatus_ValidRoadIdAndDateRange_ReturnsExpectedResult()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[{\"displayName\":\"A2\",\"statusSeverity\":\"Good\",\"statusSeverityDescription\":\"No Exceptional Delays\"}]"),
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new RoadsStatusService("mockAPIKey", httpClient);

            var result = await service.GetRoadStatus("A2", "2022-11-10", "2022-11-18");

            Assert.Contains("A2", result);
            Assert.Contains("Good", result);
        }

        // If an invalid ID is given, an exception should be thrown.
        [Fact]
        public async Task GetRoadStatus_InvalidRoadId_ThrowsException()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("{\"message\":\"An error occurred: The following road id is not recognised: a233\"}"),
            };

            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var service = new RoadsStatusService("mockAPIKey", httpClient);

            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetRoadStatus("InvalidRoadId", "2022-11-10", "2022-11-18"));
        }
    }
}