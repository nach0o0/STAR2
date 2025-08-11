using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;

namespace WpfClient.Services.Application.PermissionAdmin
{
    public interface IPermissionAdminService
    {
        Task<List<RoleModel>> GetRolesByScopeAsync(string scope);
        Task<List<PermissionModel>> GetAllPermissionsAsync();
        Task<List<PermissionModel>> GetPermissionsByScopeAsync(string scope);
        Task<RoleModel?> CreateRoleAsync(string name, string description, string scope);
        Task UpdateRoleAsync(Guid roleId, string name, string description);
        Task DeleteRoleAsync(Guid roleId);
        Task AddPermissionToRoleAsync(Guid roleId, string permissionId);
        Task RemovePermissionFromRoleAsync(Guid roleId, string permissionId);
        Task<UserAssignmentsModel> GetAssignmentsForUserInScopeAsync(Guid userId, string scope);
        Task AssignRoleToUserAsync(Guid userId, Guid roleId, string scope);
        Task RemoveRoleFromUserAsync(Guid userId, Guid roleId, string scope);
        Task AssignDirectPermissionToUserAsync(Guid userId, string permissionId, string scope);
        Task RemoveDirectPermissionFromUserAsync(Guid userId, string permissionId, string scope);
    }
}
