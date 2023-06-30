using Model.Cooperative;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.Cooperative.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    namespace Business.Cooperative.Api
    {
        public class ApiGoatFarm : IFarm<Goat>
        {
             HttpClient _httpClient = ApiClient.GetClient();

            public ApiGoatFarm(HttpClient httpClient)
            {
                 
            }

            public async Task<(List<(Goat, Goat)> eligiblePairs, List<Goat> pregnantGoats, string message)> BreedLivestockAsync(int idCoop)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/livestock/breed?idCoop={idCoop}");
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<(List<(Goat, Goat)>, List<Goat>, string)>(jsonResponse);
                return result;
            }

            public async Task<List<Goat>> ListAvailableLivestock(int coopId)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"livestock/available/{coopId}");
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Goat>>(jsonResponse);
                return result;
            }

            public async Task<(List<Goat> allNewBorn, List<Goat> pregnantGoats, string message)> HandleBirth(int idCoop, List<LivestockGender> kidGenders, List<string> kidNames)
            {
                var request = new
                {
                    IdCoop = idCoop,
                    KidGenders = kidGenders,
                    KidNames = kidNames
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/livestock/naming", content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<(List<Goat>, List<Goat>, string)>(jsonResponse);
                return result;
            }

            public async Task<Goat> EatLivestock(string eatenGoatName, int idCoop)
            {
                HttpResponseMessage response = await _httpClient.PostAsync($"api/livestock/eat?eatenGoatName={eatenGoatName}&idCoop={idCoop}", null);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Goat>(jsonResponse);
                return result;
            }

            public async Task<(List<Goat> goatsToKeep, double totalPrice)> OptimizeHerdGrowthAsync(bool extendGenetics, int malesToKeep, bool sellGoats, double sellPrice, string goatName, int idCoop)
            {
                var request = new
                {
                    ExtendGenetics = extendGenetics,
                    MalesToKeep = malesToKeep,
                    SellGoats = sellGoats,
                    SellPrice = sellPrice,
                    GoatName = goatName,
                    IdCoop = idCoop
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/livestock/optimize", content);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<(List<Goat>, double)>(jsonResponse);
                return result;
            }

            public string BuyLivestock(string name, string genderInput, string input, double price, int idCoop, ref double totalPrice)
            {
                throw new NotImplementedException();
            }
        }
    }
}
