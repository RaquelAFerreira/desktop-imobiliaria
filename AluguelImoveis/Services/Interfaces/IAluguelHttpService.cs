using AluguelImoveis.Models;
using AluguelImoveis.Models.DTOs;
using System.Net.Http;

public interface IAluguelHttpService
{
    Task<List<AluguelDetalhadoDto>> GetAllAsync();
    Task<HttpResponseMessage> CreateAsync(Aluguel aluguel);
    Task DeleteAsync(Guid id);
}