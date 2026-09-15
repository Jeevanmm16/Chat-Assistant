using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace <ProjectName>Core.Integration
{
    public interface IExternalApiService
    {
        Task<string> FetchDataAsync();
    }

    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        // HttpClient is injected via Typed Client pattern from IHttpClientFactory
        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> FetchDataAsync()
        {
            var response = await _httpClient.GetAsync("/api/data");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
