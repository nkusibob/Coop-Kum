using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Business.Cooperative.Api;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Business.Cooperative.Tests
{
    class FakeHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;
        public FakeHandler(HttpResponseMessage response) => _response = response;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(_response);
    }

    public class ApiClientSimulationTests
    {
        [Fact]
        public async Task CallApiSimulationAsync_NonSuccess_ThrowsHttpRequestException()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("server error")
            };
            var client = new HttpClient(new FakeHandler(response)) { BaseAddress = new System.Uri("https://api/") };
            var api = new ApiClientSimulation(client, NullLogger<ApiClientSimulation>.Instance);

            await Assert.ThrowsAsync<HttpRequestException>(() => api.CallApiSimulationAsync(new Business.Cooperative.Goal()));
        }

        [Fact]
        public async Task CallApiSimulationAsync_NullJson_ThrowsInvalidOperationException()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null", System.Text.Encoding.UTF8, "application/json")
            };
            var client = new HttpClient(new FakeHandler(response)) { BaseAddress = new System.Uri("https://api/") };
            var api = new ApiClientSimulation(client, NullLogger<ApiClientSimulation>.Instance);

            await Assert.ThrowsAsync<InvalidOperationException>(() => api.CallApiSimulationAsync(new Business.Cooperative.Goal()));
        }

        [Fact]
        public async Task CallApiSimulationAsync_ValidResponse_ReturnsProjectProduction()
        {
            var json = "{\"globalProjectedBenefit\":123.45,\"projectionsPerYear\":[{\"projectName\":\"P\",\"generatedProduction\":100,\"numberOfMonth\":12}]}";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
            var client = new HttpClient(new FakeHandler(response)) { BaseAddress = new System.Uri("https://api/") };
            var api = new ApiClientSimulation(client, NullLogger<ApiClientSimulation>.Instance);

            var result = await api.CallApiSimulationAsync(new Business.Cooperative.Goal());
            Assert.NotNull(result);
            Assert.Equal(123.45m, result.globalProjectedBenefit);
            Assert.NotNull(result.projectionsPerYear);
            Assert.Single(result.projectionsPerYear);
        }
    }
}
