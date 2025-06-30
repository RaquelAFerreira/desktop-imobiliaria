using AluguelImoveis.Models;
using System.Net.Http;

namespace AluguelImoveis.Services.Interfaces
{
    public interface ILocatarioHttpService
    {
        Task<List<Locatario>> GetAllAsync();
        Task<HttpResponseMessage> CreateAsync(Locatario locatario);
        Task<HttpResponseMessage> UpdateAsync(Locatario locatario);
        Task DeleteAsync(Guid id);
    }
}
