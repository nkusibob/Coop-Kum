using Business.Cooperative.BusinessModel;
using Business.Cooperative.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Business.Cooperative.Api
{
    internal class ApiClientNonstatic : IApiClientNonStatic
    {
        private readonly HttpClient _http;

        public ApiClientNonstatic(HttpClient http)
        {
            _http = http;
        }
        public async Task CreateEmployeasync(CreateEmployeeStepCommand employeeDTo)
        {
            var response = await _http.PostAsJsonAsync("/employees", employeeDTo);
            response.EnsureSuccessStatusCode();
        }

        public async Task<EmployeeCreateDataDto> GetEmployeeCreateDataAsync(int projectId, int managerId)
        {
            var response = await _http.GetAsync(
            $"api/projects/{projectId}/managers/{managerId}/create-data");

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(
                    $"API call failed: {response.StatusCode}");
            }

            var data = await response.Content
                .ReadFromJsonAsync<EmployeeCreateDataDto>();

            if (data == null)
                throw new InvalidOperationException("API returned null data.");

            return data;

        }
    }
}
