using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.Interfaces
{
    public interface IOrganizationApiClient
    {
        [Post("/api/organizations")]
        Task<CreateOrganizationResponse> CreateOrganizationAsync([Body] CreateOrganizationRequest request);

        [Put("/api/organizations/{organizationId}")]
        Task UpdateOrganizationAsync(Guid organizationId, [Body] UpdateOrganizationRequest request);

        [Delete("/api/organizations/{organizationId}")]
        Task DeleteOrganizationAsync(Guid organizationId, [Query] bool deleteSubOrganizations);

        [Patch("/api/organizations/{organizationId}/parent")]
        Task ReassignOrganizationParentAsync(Guid organizationId, [Body] ReassignOrganizationToParentRequest request);

        [Get("/api/organizations/{organizationId}/employees")]
        Task<List<EmployeeResponse>> GetEmployeesByOrganizationAsync(Guid organizationId);


        [Get("/api/employees/me")]
        Task<MyEmployeeProfileResponse?> GetMyEmployeeProfileAsync();

        [Post("/api/employees/me")]
        Task<CreateEmployeeResponse> CreateMyEmployeeProfileAsync([Body] CreateEmployeeRequest request);

        [Put("/api/employees/me")]
        Task UpdateMyEmployeeProfileAsync([Body] UpdateEmployeeRequest request);


        [Post("/api/employeegroups")]
        Task<CreateEmployeeGroupResponse> CreateEmployeeGroupAsync([Body] CreateEmployeeGroupRequest request);

        [Put("/api/employeegroups/{employeeGroupId}")]
        Task UpdateEmployeeGroupAsync(Guid employeeGroupId, [Body] UpdateEmployeeGroupRequest request);

        [Delete("/api/employeegroups/{employeeGroupId}")]
        Task DeleteEmployeeGroupAsync(Guid employeeGroupId);

        [Patch("/api/employeegroups/{employeeGroupId}/transfer")]
        Task TransferEmployeeGroupAsync(Guid employeeGroupId, [Body] TransferEmployeeGroupRequest request);


        [Delete("/api/employees/{employeeId}/organization")]
        Task RemoveEmployeeFromOrganizationAsync(Guid employeeId);

        [Post("/api/employeegroups/{employeeGroupId}/members")]
        Task AddEmployeeToGroupAsync(Guid employeeGroupId, [Body] AddEmployeeToGroupRequest request);

        [Delete("/api/employeegroups/{employeeGroupId}/members/{employeeId}")]
        Task RemoveEmployeeFromGroupAsync(Guid employeeGroupId, Guid employeeId);

        [Put("/api/employees/{employeeId}/groups/{employeeGroupId}/hourly-rate")]
        Task AssignHourlyRateToEmployeeAsync(Guid employeeId, Guid employeeGroupId, [Body] AssignHourlyRateToEmployeeRequest request);


        [Post("/api/invitations")]
        Task<CreateInvitationResponse> CreateInvitationAsync([Body] CreateInvitationRequest request);

        [Post("/api/invitations/{invitationId}/accept")]
        Task AcceptInvitationAsync(Guid invitationId);

        [Post("/api/invitations/{invitationId}/decline")]
        Task DeclineInvitationAsync(Guid invitationId);

        [Delete("/api/invitations/{invitationId}")]
        Task RevokeInvitationAsync(Guid invitationId);


        [Post("/api/hourlyrates")]
        Task<CreateHourlyRateResponse> CreateHourlyRateAsync([Body] CreateHourlyRateRequest request);

        [Put("/api/hourlyrates/{hourlyRateId}")]
        Task UpdateHourlyRateAsync(Guid hourlyRateId, [Body] UpdateHourlyRateRequest request);

        [Delete("/api/hourlyrates/{hourlyRateId}")]
        Task DeleteHourlyRateAsync(Guid hourlyRateId);
    }
}
