using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Shared.Application.Interfaces.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using static Shared.Application.Interfaces.Infrastructure.IOrganizationServiceClient;

namespace Shared.Clients
{
    public class OrganizationServiceClient : IOrganizationServiceClient
    {
        private readonly HttpClient _httpClient;

        public OrganizationServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<(Guid EmployeeId, Guid? OrganizationId, List<Guid> EmployeeGroupIds)?> GetEmployeeInfoByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            // Annahme: Es gibt einen internen Endpunkt im Organization Service, der diese Abfrage unterstützt.
            var httpResponse = await _httpClient.GetAsync($"api/internal/employee-info/by-employee/{employeeId}", cancellationToken);
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            httpResponse.EnsureSuccessStatusCode();
            var responseDto = await httpResponse.Content.ReadFromJsonAsync<EmployeeInfoResponse>(cancellationToken);
            return (responseDto!.EmployeeId, responseDto.OrganizationId, responseDto.EmployeeGroupIds);
        }

        public async Task<List<EmployeeDetailsResponse>> GetEmployeesByUserIdsAsync(
            GetEmployeesByUserIdsRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("api/internal/employees/by-user-ids", request, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<EmployeeDetailsResponse>>(cancellationToken: cancellationToken)
                   ?? new List<EmployeeDetailsResponse>();
        }

        public async Task<List<EmployeeResponse>> GetEmployeesByEmployeeGroupAsync(Guid employeeGroupId, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync($"api/employee-groups/{employeeGroupId}/employees", cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<EmployeeResponse>>(cancellationToken: cancellationToken)
                   ?? new List<EmployeeResponse>();
        }

        public async Task<List<EmployeeDto>> GetEmployeesByIdsAsync(List<Guid> employeeIds, CancellationToken cancellationToken = default)
        {
            if (employeeIds == null || !employeeIds.Any())
            {
                return new List<EmployeeDto>();
            }

            // Dieser Endpunkt muss im OrganizationService erstellt werden
            var response = await _httpClient.PostAsJsonAsync("/api/internal/employees/by-ids", new { employeeIds }, cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<EmployeeDto>>() ?? new List<EmployeeDto>();
        }
    }
}
