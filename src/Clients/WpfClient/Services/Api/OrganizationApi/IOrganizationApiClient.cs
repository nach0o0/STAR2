using Organization.Contracts.Requests;
using Organization.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.OrganizationApi
{
    public interface IOrganizationApiClient
    {
        // Organization
        Task<CreateOrganizationResponse> CreateOrganizationAsync(CreateOrganizationRequest request);
        Task UpdateOrganizationAsync(Guid organizationId, UpdateOrganizationRequest request);
        Task DeleteOrganizationAsync(Guid organizationId, bool deleteSubOrganizations);
        Task ReassignOrganizationParentAsync(Guid organizationId, ReassignOrganizationToParentRequest request);

        // Employee
        Task<MyEmployeeProfileResponse?> GetMyEmployeeProfileAsync();
        Task<CreateEmployeeResponse> CreateMyEmployeeProfileAsync(CreateEmployeeRequest request);
        Task UpdateMyEmployeeProfileAsync(UpdateEmployeeRequest request);
        //Task<List<EmployeeSearchResultResponse>> SearchEmployeesAsync(string searchTerm);

        // Employee Group
        Task<CreateEmployeeGroupResponse> CreateEmployeeGroupAsync(CreateEmployeeGroupRequest request);
        Task UpdateEmployeeGroupAsync(Guid employeeGroupId, UpdateEmployeeGroupRequest request);
        Task DeleteEmployeeGroupAsync(Guid employeeGroupId);
        Task TransferEmployeeGroupAsync(Guid employeeGroupId, TransferEmployeeGroupRequest request);

        // Employee Membership
        Task RemoveEmployeeFromOrganizationAsync(Guid employeeId);
        Task AddEmployeeToGroupAsync(Guid employeeGroupId, AddEmployeeToGroupRequest request);
        Task RemoveEmployeeFromGroupAsync(Guid employeeGroupId, Guid employeeId);
        Task AssignHourlyRateToEmployeeAsync(Guid employeeId, Guid employeeGroupId, AssignHourlyRateToEmployeeRequest request);

        // Invitation
        Task<CreateInvitationResponse> CreateInvitationAsync(CreateInvitationRequest request);
        Task AcceptInvitationAsync(Guid invitationId);
        Task DeclineInvitationAsync(Guid invitationId);
        Task RevokeInvitationAsync(Guid invitationId);

        // Hourly Rate
        Task<CreateHourlyRateResponse> CreateHourlyRateAsync(CreateHourlyRateRequest request);
        Task UpdateHourlyRateAsync(Guid hourlyRateId, UpdateHourlyRateRequest request);
        Task DeleteHourlyRateAsync(Guid hourlyRateId);
    }
}
