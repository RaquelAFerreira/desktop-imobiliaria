using AluguelImoveis.Models;
using System.Net.Http;

namespace AluguelImoveis.Services.Interfaces
{
    public interface IImovelHttpService
    {
        Task<List<Imovel>> GetAllAsync();
        Task<List<Imovel>> GetDisponiveisAsync();
        Task<HttpResponseMessage> CreateAsync(Imovel imovel);
        Task<HttpResponseMessage> UpdateAsync(Imovel imovel);
        Task DeleteAsync(Guid id);
    }

}
