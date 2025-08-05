using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.OrganizationApi
{
    public class OrganizationApiClient : IOrganizationApiClient
    {
        private readonly HttpClient _httpClient;

        public OrganizationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Organization
        public async Task<CreateOrganizationResponse> CreateOrganizationAsync(CreateOrganizationRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/organizations", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<CreateOrganizationResponse>();
        }

        public async Task UpdateOrganizationAsync(Guid organizationId, UpdateOrganizationRequest request)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync($"api/organizations/{organizationId}", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task DeleteOrganizationAsync(Guid organizationId, bool deleteSubOrganizations)
        {
            var httpResponse = await _httpClient.DeleteAsync($"api/organizations/{organizationId}?deleteSubOrganizations={deleteSubOrganizations}");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task ReassignOrganizationParentAsync(Guid organizationId, ReassignOrganizationToParentRequest request)
        {
            var httpResponse = await _httpClient.PatchAsJsonAsync($"api/organizations/{organizationId}/parent", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        // Employee
        public async Task<MyEmployeeProfileResponse?> GetMyEmployeeProfileAsync()
        {
            var httpResponse = await _httpClient.GetAsync("api/employees/me");

            // Wenn kein Profil existiert (404), gib einfach null zurück, das ist kein Fehler.
            if (httpResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            // Bei allen anderen Fehlern, wirf eine Exception.
            await httpResponse.EnsureSuccessOrThrowApiException();

            return await httpResponse.Content.ReadFromJsonAsync<MyEmployeeProfileResponse>();
        }

        public async Task<CreateEmployeeResponse> CreateMyEmployeeProfileAsync(CreateEmployeeRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/employees/me", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<CreateEmployeeResponse>();
        }

        public async Task UpdateMyEmployeeProfileAsync(UpdateEmployeeRequest request)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync("api/employees/me", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        //public async Task<List<EmployeeSearchResultResponse>> SearchEmployeesAsync(string searchTerm)
        //{
        //    return await _httpClient.GetFromJsonAsync<List<EmployeeSearchResultResponse>>($"api/employees/search?searchTerm={searchTerm}");
        //}

        // Employee Group
        public async Task<CreateEmployeeGroupResponse> CreateEmployeeGroupAsync(CreateEmployeeGroupRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/employeegroups", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<CreateEmployeeGroupResponse>();
        }

        public async Task UpdateEmployeeGroupAsync(Guid employeeGroupId, UpdateEmployeeGroupRequest request)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync($"api/employeegroups/{employeeGroupId}", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task DeleteEmployeeGroupAsync(Guid employeeGroupId)
        {
            var httpResponse = await _httpClient.DeleteAsync($"api/employeegroups/{employeeGroupId}");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task TransferEmployeeGroupAsync(Guid employeeGroupId, TransferEmployeeGroupRequest request)
        {
            var httpResponse = await _httpClient.PatchAsJsonAsync($"api/employeegroups/{employeeGroupId}/transfer", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        // Employee Membership
        public async Task RemoveEmployeeFromOrganizationAsync(Guid employeeId)
        {
            var httpResponse = await _httpClient.DeleteAsync($"api/employees/{employeeId}/organization");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task AddEmployeeToGroupAsync(Guid employeeGroupId, AddEmployeeToGroupRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync($"api/employeegroups/{employeeGroupId}/members", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task RemoveEmployeeFromGroupAsync(Guid employeeGroupId, Guid employeeId)
        {
            var httpResponse = await _httpClient.DeleteAsync($"api/employeegroups/{employeeGroupId}/members/{employeeId}");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task AssignHourlyRateToEmployeeAsync(Guid employeeId, Guid employeeGroupId, AssignHourlyRateToEmployeeRequest request)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync($"api/employees/{employeeId}/groups/{employeeGroupId}/hourly-rate", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        // Invitation
        public async Task<CreateInvitationResponse> CreateInvitationAsync(CreateInvitationRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/invitations", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<CreateInvitationResponse>();
        }

        public async Task AcceptInvitationAsync(Guid invitationId)
        {
            var httpResponse = await _httpClient.PostAsync($"api/invitations/{invitationId}/accept", null);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task DeclineInvitationAsync(Guid invitationId)
        {
            var httpResponse = await _httpClient.PostAsync($"api/invitations/{invitationId}/decline", null);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task RevokeInvitationAsync(Guid invitationId)
        {
            var httpResponse = await _httpClient.DeleteAsync($"api/invitations/{invitationId}");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        // Hourly Rate
        public async Task<CreateHourlyRateResponse> CreateHourlyRateAsync(CreateHourlyRateRequest request)
        {
            var httpResponse = await _httpClient.PostAsJsonAsync("api/hourlyrates", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
            return await httpResponse.Content.ReadFromJsonAsync<CreateHourlyRateResponse>();
        }

        public async Task UpdateHourlyRateAsync(Guid hourlyRateId, UpdateHourlyRateRequest request)
        {
            var httpResponse = await _httpClient.PutAsJsonAsync($"api/hourlyrates/{hourlyRateId}", request);
            await httpResponse.EnsureSuccessOrThrowApiException();
        }

        public async Task DeleteHourlyRateAsync(Guid hourlyRateId)
        {
            var httpResponse = await _httpClient.DeleteAsync($"api/hourlyrates/{hourlyRateId}");
            await httpResponse.EnsureSuccessOrThrowApiException();
        }
    }
}
