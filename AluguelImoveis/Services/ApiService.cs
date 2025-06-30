using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

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
            return await _client.PostAsJsonAsync("Locatarios", locatario);
        }

        public static async Task<HttpResponseMessage> CriarImovelAsync(Imovel imovel)
        {
            return await _client.PostAsJsonAsync("Imoveis", imovel);
        }

        public static async Task<List<AluguelDetalhadoDto>> GetAlugueisAsync()
        {
            return await _client.GetFromJsonAsync<List<AluguelDetalhadoDto>>("Alugueis");
        }

        public static async Task<List<Imovel>> GetImoveisDisponiveisAsync()
        {
            return await _client.GetFromJsonAsync<List<Imovel>>("Imoveis/disponiveis");
        }

        public static async Task<HttpResponseMessage> CriarAluguelAsync(Aluguel aluguel)
        {
            var response = await _client.PostAsJsonAsync("Alugueis", aluguel);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static async Task ExcluirImovelAsync(Guid imovelId)
        {
            var response = await _client.DeleteAsync($"Imoveis/{imovelId}");
            response.EnsureSuccessStatusCode();
        }

        public static async Task ExcluirLocatarioAsync(Guid locatarioId)
        {
            var response = await _client.DeleteAsync($"Locatarios/{locatarioId}");
            response.EnsureSuccessStatusCode();
        }
        public static async Task ExcluirAluguelAsync(Guid aluguelId)
        {
            var response = await _client.DeleteAsync($"Alugueis/{aluguelId}");
            response.EnsureSuccessStatusCode();
        }

        public static async Task<HttpResponseMessage> AtualizarImovelAsync(Imovel imovel)
        {
            var response = await _client.PutAsJsonAsync($"Imoveis/{imovel.Id}", imovel);
            response.EnsureSuccessStatusCode();
            return response;
        }

    }
}
