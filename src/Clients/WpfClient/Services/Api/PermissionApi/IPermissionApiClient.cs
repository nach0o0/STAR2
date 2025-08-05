using Permission.Contracts.Requests;
using Permission.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.PermissionApi
{
    public interface IPermissionApiClient
    {
        // Roles
        Task<CreateRoleResponse> CreateRoleAsync(CreateRoleRequest request);
        Task UpdateRoleAsync(Guid roleId, UpdateRoleRequest request);
        Task DeleteRoleAsync(Guid roleId);

        // Permissions on Roles
        Task AddPermissionToRoleAsync(Guid roleId, AddPermissionToRoleRequest request);
        Task RemovePermissionFromRoleAsync(Guid roleId, string permissionId);

        // Assignments
        Task<AssignRoleToUserResponse> AssignRoleToUserAsync(AssignRoleToUserRequest request);
        Task RemoveRoleFromUserAsync(RemoveRoleFromUserRequest request);
        Task<AssignDirectPermissionToUserResponse> AssignDirectPermissionToUserAsync(AssignDirectPermissionToUserRequest request);
        Task RemoveDirectPermissionFromUserAsync(RemoveDirectPermissionFromUserRequest request);
    }
}
