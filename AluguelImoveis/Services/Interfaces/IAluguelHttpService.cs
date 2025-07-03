using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using System.Net.Http;

public interface IAluguelHttpService
{
    Task<List<AluguelDto>> GetAllAsync();
    Task<HttpResponseMessage> CreateAsync(Aluguel aluguel);
    Task DeleteAsync(Guid id);
}