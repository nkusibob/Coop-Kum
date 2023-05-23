using System.Net.Http.Headers;
using System.Net.Http;
using System;

namespace Business.Cooperative.Api
{
    public static class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        static ApiClient()
        {
            client.BaseAddress = new Uri("http://localhost:6846/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static HttpClient GetClient()
        {
            return client;
        }
    }
}
