using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using AluguelImoveis.Services.Interfaces;
using System.Net.Http;

public class AluguelHttpService : IAluguelHttpService
{
    private readonly IHttpService _httpService;
    private const string Endpoint = "Alugueis";

    public AluguelHttpService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public Task<List<AluguelDto>> GetAllAsync() =>
        _httpService.GetAsync<List<AluguelDto>>(Endpoint);

    public Task<HttpResponseMessage> CreateAsync(Aluguel aluguel) =>
        _httpService.PostAsync(Endpoint, aluguel);

    public Task DeleteAsync(Guid id) => _httpService.DeleteAsync($"{Endpoint}/{id}");
}