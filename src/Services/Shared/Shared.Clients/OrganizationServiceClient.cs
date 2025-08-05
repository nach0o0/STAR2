using Microsoft.Extensions.Logging;
using Organization.Contracts.Responses;
using Shared.Application.Interfaces.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Shared.Clients
{
    public class OrganizationServiceClient : IOrganizationServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<OrganizationServiceClient> _logger;

        public OrganizationServiceClient(HttpClient httpClient, ILogger<OrganizationServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<(Guid EmployeeId, Guid? OrganizationId, List<Guid> EmployeeGroupIds)?> GetEmployeeInfoByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.GetAsync(
                $"api/internal/employee-info/by-user/{userId}",
                cancellationToken);

            // Wenn der Statuscode 404 ist, gib einfach null zurück. Das ist kein Fehler.
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // Wirf nur bei anderen Fehlern eine Exception.
            httpResponse.EnsureSuccessStatusCode();

            var responseDto = await httpResponse.Content.ReadFromJsonAsync<EmployeeInfoResponse>(cancellationToken);

            return (responseDto!.EmployeeId, responseDto.OrganizationId, responseDto.EmployeeGroupIds);
        }
    }
}
