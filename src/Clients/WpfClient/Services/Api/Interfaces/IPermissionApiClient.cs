using Permission.Contracts.Requests;
using Refit;
using Permission.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Api.Interfaces
{
    public interface IPermissionApiClient
    {
        [Get("/api/roles")]
        Task<List<RoleResponse>> GetRolesByScopeAsync([Query] string scope);

        [Post("/api/roles")]
        Task<CreateRoleResponse> CreateRoleAsync([Body] CreateRoleRequest request);

        [Put("/api/roles/{roleId}")]
        Task UpdateRoleAsync(Guid roleId, [Body] UpdateRoleRequest request);

        [Delete("/api/roles/{roleId}")]
        Task DeleteRoleAsync(Guid roleId);

        [Post("/api/roles/{roleId}/permissions")]
        Task AddPermissionToRoleAsync(Guid roleId, [Body] AddPermissionToRoleRequest request);

        [Delete("/api/roles/{roleId}/permissions/{permissionId}")]
        Task RemovePermissionFromRoleAsync(Guid roleId, string permissionId);


        [Get("/api/permissions")]
        Task<List<PermissionResponse>> GetPermissionsByScopeAsync([Query] string scope);

        [Get("/api/roles/{roleId}/permissions")]
        Task<List<PermissionResponse>> GetPermissionsByRoleAsync(Guid roleId);


        [Get("/api/user-permissions/user")]
        Task<UserAssignmentsResponse> GetAssignmentsForUserInScopeAsync([Query] Guid userId, [Query] string scope);

        [Get("/api/user-permissions/by-scope")]
        Task<AssignmentsByScopeResponse> GetAssignmentsByScopeAsync([Query] string scope);

        [Post("/api/user-permissions/roles")]
        Task<AssignRoleToUserResponse> AssignRoleToUserAsync([Body] AssignRoleToUserRequest request);

        [Delete("/api/user-permissions/roles")]
        Task RemoveRoleFromUserAsync([Body] RemoveRoleFromUserRequest request);

        [Post("/api/user-permissions/direct")]
        Task<AssignDirectPermissionToUserResponse> AssignDirectPermissionToUserAsync([Body] AssignDirectPermissionToUserRequest request);

        [Delete("/api/user-permissions/direct")]
        Task RemoveDirectPermissionFromUserAsync([Body] RemoveDirectPermissionFromUserRequest request);
    }
}
