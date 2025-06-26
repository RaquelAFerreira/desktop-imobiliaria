using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AluguelImoveis.Models;

namespace AluguelImoveis.Services
{
    public static class ApiService
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new System.Uri("http://localhost:5287/api/")
        };

        public static async Task<List<Locatario>> GetLocatariosAsync()
        {
            return await _client.GetFromJsonAsync<List<Locatario>>("Locatarios");
        }

        public static async Task<List<Imovel>> GetImoveisAsync()
        {
            return await _client.GetFromJsonAsync<List<Imovel>>("Imoveis");
        }

        public static async Task<HttpResponseMessage> CriarLocatarioAsync(Locatario locatario)
        {
            return await _client.PostAsJsonAsync("locatarios", locatario);
        }
    }
}
