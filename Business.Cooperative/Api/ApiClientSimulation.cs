using Microsoft.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Business.Cooperative.Api
{
    public class ApiClientSimulation : IBusinessApiCallLogic
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiClientSimulation> _logger;
        public ApiClientSimulation(HttpClient httpClient, Microsoft.Extensions.Logging.ILogger<ApiClientSimulation> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProjectProduction> CallApiSimulationAsync(Goal goal)
        {
            var json = JsonConvert.SerializeObject(goal);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
            
                var response = await _httpClient.PostAsync("api/SimulationPlan/Simulation", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Simulation API returned {(int)response.StatusCode}: {responseContent}");
                }

                var result = JsonConvert.DeserializeObject<ProjectProduction>(responseContent);

                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize ProjectProduction from simulation API response. Response content: " + responseContent);
                }

                return result;
            }
            catch (Exception ex)
            {
                // Preserve original exception information for diagnostics
                _logger.LogError(ex, "API call failed.");
                throw;
            }
        }

        async Task<ProjectProduction> IBusinessApiCallLogic.CallApiProductionPlanAsync(ProjectionPerPeriod prjperiod)
        {
            var json = JsonConvert.SerializeObject(prjperiod);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/ProductionPlan/projection", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Production API returned {(int)response.StatusCode}: {responseContent}");
                }

                var result = JsonConvert.DeserializeObject<ProjectProduction>(responseContent);

                if (result == null)
                {
                    throw new InvalidOperationException("Failed to deserialize ProjectProduction from production API response. Response content: " + responseContent);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "API call failed.");
                throw;
            }
        }
    }
}