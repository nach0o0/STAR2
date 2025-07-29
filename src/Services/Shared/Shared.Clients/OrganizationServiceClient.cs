using Microsoft.Extensions.Logging;
using Organization.Contracts.Responses;
using Shared.Application.Interfaces.Infrastructure;
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

        public async Task<(Guid EmployeeId, Guid OrganizationId, List<Guid> EmployeeGroupIds)?> GetEmployeeInfoByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var responseDto = await _httpClient.GetFromJsonAsync<EmployeeInfoResponse>(
                $"api/internal/employee-info/by-user/{userId}",
                cancellationToken);

            if (responseDto is null) return null;

            return (responseDto.EmployeeId, responseDto.OrganizationId, responseDto.EmployeeGroupIds);
        }
    }
}
