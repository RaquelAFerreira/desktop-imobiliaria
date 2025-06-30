using AluguelImoveis.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AluguelImoveis.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await _client.GetFromJsonAsync<T>(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var response = await _client.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            var response = await _client.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _client.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}