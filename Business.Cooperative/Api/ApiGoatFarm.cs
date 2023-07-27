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
    using System.Reflection;
    using System.Threading.Tasks;
    using global::Business.Cooperative.Api.RequestModel;
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

            public async Task<(List<Goat> goatsToKeep, decimal totalPrice)> OptimizeHerdGrowthAsync(bool extendGenetics, int malesToKeep, bool sellGoats, decimal sellPrice, string goatName, int idCoop)
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
                var result = JsonConvert.DeserializeObject<(List<Goat>, decimal)>(jsonResponse);
                return result;
            }
            private LivestockGender ParseGender(string genderInput)
            {
                if (genderInput == "male")
                {
                    return LivestockGender.Male;
                }
                else if (genderInput.ToLower() == "female")
                {
                    return LivestockGender.Female;
                }
                else
                {
                    // Handle invalid gender input
                    throw new ArgumentException("Invalid gender input.");
                }
            }

            public async Task<string> BuyLivestock(string name, string genderInput, string input, decimal price, int idCoop, decimal totalPrice)
            {
                DateTime birthdate;
                int age;
                if (DateTime.TryParse(input, out birthdate))
                {
                    age = DateTime.Now.Year - birthdate.Year;
                    if (DateTime.Now < birthdate.AddYears(age))
                    {
                        age--;
                    }
                }
                else if (double.TryParse(input, out var doubleAge))
                {
                    age = (int)doubleAge;
                }
                else
                {
                    throw new ArgumentException("Invalid input. Enter a valid birthdate of the goat (yyyy-MM-dd) or its age as double.");
                }
                // Create a new instance of the Goat class with the provided details
                var goat = new Goat(name, ParseGender(genderInput), age, idCoop, price);



                // Serialize the goat object to JSON
                var jsonRequest = JsonConvert.SerializeObject(goat);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                // Make the HTTP POST request to the endpoint
                var response = await _httpClient.PostAsync("livestock/buy", content);
                response.EnsureSuccessStatusCode();

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // Continue with the rest of the buy livestock logic
                // ...

                // Update the totalPrice if needed
                totalPrice += price;

                // Return the updated totalPrice
                return responseContent;
            }


            public async Task<Goat> UpdateDetails(int livestockId, Goat goat)
            {
                string jsonRequest = JsonConvert.SerializeObject(goat);
                HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"Livestock/details/{livestockId}", content);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var updatedGoat = JsonConvert.DeserializeObject<Goat>(jsonResponse);

                return updatedGoat;
            }

            public async Task<Goat> UpdateDetails(int livestockId)
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"livestock/details/{livestockId}");
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var goatDetails = JsonConvert.DeserializeObject<Goat>(jsonResponse);
                return goatDetails;
            }

            Task<string> IFarm<Goat>.BuyLivestock(Goat goat)
            {
                throw new NotImplementedException();
            }
        }
    }
}
