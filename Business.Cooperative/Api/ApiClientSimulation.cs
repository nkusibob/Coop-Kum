using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Business.Cooperative.Api
{
    public class ApiClientSimulation : IBusinessApiCallLogic
    {
        private readonly HttpClient _httpClient;

        public ApiClientSimulation(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }
        public async Task<ProjectProduction> CallApiSimulationAsync(Goal goal)
        {
            var json = JsonConvert.SerializeObject(goal);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("SimulationPlan/Simulation", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProjectProduction>(responseContent);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("API call failed."+ex.Message);
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
                var result = JsonConvert.DeserializeObject<ProjectProduction>(responseContent);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception("API call failed." + ex.Message);
            }
        }

      
    }
}