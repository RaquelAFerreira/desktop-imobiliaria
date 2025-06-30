using AluguelImoveis.Models;
using AluguelImoveis.Services.Interfaces;
using System.Net.Http;

namespace AluguelImoveis.Services
{
    public class ImovelHttpService : IImovelHttpService
    {
        private readonly IHttpService _httpService;
        private const string Endpoint = "Imoveis";

        public ImovelHttpService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public Task<List<Imovel>> GetAllAsync() => _httpService.GetAsync<List<Imovel>>(Endpoint);

        public Task<List<Imovel>> GetDisponiveisAsync() =>
            _httpService.GetAsync<List<Imovel>>($"{Endpoint}/disponiveis");

        public Task<HttpResponseMessage> CreateAsync(Imovel imovel) =>
            _httpService.PostAsync(Endpoint, imovel);

        public Task<HttpResponseMessage> UpdateAsync(Imovel imovel) =>
            _httpService.PutAsync($"{Endpoint}/{imovel.Id}", imovel);

        public Task DeleteAsync(Guid id) => _httpService.DeleteAsync($"{Endpoint}/{id}");
    }
}
