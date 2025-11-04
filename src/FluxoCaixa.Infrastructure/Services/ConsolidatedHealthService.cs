using System.Net.Http;
using System.Threading.Tasks;

namespace FluxoCaixa.Infrastructure.Services
{
    public interface IConsolidatedHealthService
    {
        Task<bool> IsAvailableAsync();
    }

    public class ConsolidatedHealthService : IConsolidatedHealthService
    {
        private readonly HttpClient _httpClient;

        public ConsolidatedHealthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5157"); 
        }

        public async Task<bool> IsAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/consolidated/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
