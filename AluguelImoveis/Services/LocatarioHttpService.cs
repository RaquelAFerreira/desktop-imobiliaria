using AluguelImoveis.Models;
using AluguelImoveis.Services.Interfaces;
using System.Net.Http;

namespace AluguelImoveis.Services
{
    public class LocatarioHttpService : ILocatarioHttpService
    {
        private readonly IHttpService _httpService;
        private const string Endpoint = "Locatarios";

        public LocatarioHttpService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public Task<List<Locatario>> GetAllAsync() => _httpService.GetAsync<List<Locatario>>(Endpoint);

        public Task<HttpResponseMessage> CreateAsync(Locatario locatario) =>
            _httpService.PostAsync(Endpoint, locatario);

        public Task<HttpResponseMessage> UpdateAsync(Locatario locatario) =>
            _httpService.PutAsync($"{Endpoint}/{locatario.Id}", locatario);

        public Task DeleteAsync(Guid id) => _httpService.DeleteAsync($"{Endpoint}/{id}");
    }
}
