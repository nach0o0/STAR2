using Shared.Application.Interfaces.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Clients
{
    public class CostObjectServiceClient : ICostObjectServiceClient
    {
        private readonly HttpClient _httpClient;

        public CostObjectServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CostObjectDto?> GetByIdAsync(Guid costObjectId, CancellationToken cancellationToken = default)
        {
            // Die URL muss natürlich in der Konfiguration hinterlegt werden
            return await _httpClient.GetFromJsonAsync<CostObjectDto?>($"/api/cost-objects/{costObjectId}", cancellationToken);
        }

        public async Task<List<CostObjectDto>> GetByIdsAsync(List<Guid> costObjectIds, CancellationToken cancellationToken = default)
        {
            if (costObjectIds == null || !costObjectIds.Any())
            {
                return new List<CostObjectDto>();
            }

            var response = await _httpClient.PostAsJsonAsync("/api/cost-objects/by-ids", new { costObjectIds }, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<CostObjectDto>>() ?? new List<CostObjectDto>();
        }
    }
}
