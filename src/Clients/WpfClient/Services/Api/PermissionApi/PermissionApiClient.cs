using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Permission.Contracts.Requests;
using Permission.Contracts.Responses;

namespace WpfClient.Services.Api.PermissionApi
{
    public class PermissionApiClient : IPermissionApiClient
    {
        private readonly HttpClient _httpClient;

        public PermissionApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Roles
        public async Task<CreateRoleResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/roles", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<CreateRoleResponse>();
        }

        public async Task UpdateRoleAsync(Guid roleId, UpdateRoleRequest request)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync($"api/roles/{roleId}", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var httpResponse = await _httpClient.DeleteAsync($"api/roles/{roleId}");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        // Permissions on Roles
        public async Task AddPermissionToRoleAsync(Guid roleId, AddPermissionToRoleRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync($"api/roles/{roleId}/permissions", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task RemovePermissionFromRoleAsync(Guid roleId, string permissionId)
        {
            var encodedPermissionId = HttpUtility.UrlEncode(permissionId);
            var httpResponse = await _httpClient.DeleteAsync($"api/roles/{roleId}/permissions/{encodedPermissionId}");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        // Assignments
        public async Task<AssignRoleToUserResponse> AssignRoleToUserAsync(AssignRoleToUserRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/user-permissions/roles", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<AssignRoleToUserResponse>();
        }

        public async Task RemoveRoleFromUserAsync(RemoveRoleFromUserRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "api/user-permissions/roles")
            {
                Content = JsonContent.Create(request)
            };
            var httpResponse = await _httpClient.SendAsync(httpRequest);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task<AssignDirectPermissionToUserResponse> AssignDirectPermissionToUserAsync(AssignDirectPermissionToUserRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/user-permissions/direct", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<AssignDirectPermissionToUserResponse>();
        }

        public async Task RemoveDirectPermissionFromUserAsync(RemoveDirectPermissionFromUserRequest request)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, "api/user-permissions/direct")
            {
                Content = JsonContent.Create(request)
            };
            var httpResponse = await _httpClient.SendAsync(httpRequest);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }
    }
}
