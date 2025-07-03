using AluguelImoveis.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
            var response = await _client.GetAsync(endpoint);
            await EnsureSuccessOrThrow(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var response = await _client.PostAsJsonAsync(endpoint, data);
            await EnsureSuccessOrThrow(response);
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            var response = await _client.PutAsJsonAsync(endpoint, data);
            await EnsureSuccessOrThrow(response);
            return response;
        }

        public async Task DeleteAsync(string endpoint)
        {
            var response = await _client.DeleteAsync(endpoint);
            await EnsureSuccessOrThrow(response);
        }

        private async Task EnsureSuccessOrThrow(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var content = await response.Content.ReadAsStringAsync();
            string errorMessage = "Erro ao chamar a API.";

            // Tenta interpretar como JSON
            using var json = JsonDocument.Parse(content);
            var root = json.RootElement;

            if (root.TryGetProperty("message", out var msg))
                errorMessage = msg.GetString();
            else if (root.TryGetProperty("detail", out var detail))
                errorMessage = detail.GetString();
            else if (!string.IsNullOrWhiteSpace(content))
                errorMessage = content;


            throw new ApplicationException(errorMessage);
        }
    }
}
