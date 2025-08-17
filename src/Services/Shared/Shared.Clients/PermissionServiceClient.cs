using Permission.Contracts.Requests;
using Permission.Contracts.Responses;
using Shared.Application.Interfaces.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Clients
{
    public class PermissionServiceClient : IPermissionRegistrationClient, IPermissionQueryClient
    {
        private readonly HttpClient _httpClient;

        public PermissionServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task RegisterPermissionsAsync(
            IEnumerable<(string Id, string Description, List<string> PermittedScopeTypes)> permissions,
            CancellationToken cancellationToken = default)
        {
            var permissionDtos = permissions
                .Select(p => new PermissionDto(p.Id, p.Description, p.PermittedScopeTypes))
                .ToList();
            var request = new RegisterPermissionsRequest(permissionDtos);

            var response = await _httpClient.PostAsJsonAsync("api/internal/permissions/register", request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Dictionary<string, List<string>>?> GetPermissionsForUserAsync(Guid userId, IEnumerable<string> scopes, CancellationToken cancellationToken = default)
        {
            var request = new GetUserPermissionsRequest(userId, scopes.ToList());
            var response = await _httpClient.PostAsJsonAsync("api/internal/user-permissions", request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseDto = await response.Content.ReadFromJsonAsync<GetUserPermissionsResponse>(cancellationToken);
            return responseDto?.PermissionsByScope;
        }

        public async Task<List<string>?> GetScopesForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Ruft den neuen internen Endpunkt ab, den du im PermissionService erstellt hast.
                return await _httpClient.GetFromJsonAsync<List<string>>($"api/internal/user-scopes/{userId}", cancellationToken);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<string>(); // Wenn nichts gefunden wird, gib eine leere Liste zurück.
            }
        }
    }
}
