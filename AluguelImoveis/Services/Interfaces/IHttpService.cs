using System.Net.Http;

namespace AluguelImoveis.Services.Interfaces
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data);
        Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data);
        Task DeleteAsync(string endpoint);
    }
}