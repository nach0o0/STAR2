using Permission.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Messages;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Notification;

namespace WpfClient.Services.Application.PermissionAdmin
{
    public class PermissionAdminService : IPermissionAdminService
    {
        private readonly IPermissionApiClient _permissionApiClient;
        private readonly INotificationService _notificationService;

        public PermissionAdminService(IPermissionApiClient permissionApiClient, INotificationService notificationService)
        {
            _permissionApiClient = permissionApiClient;
            _notificationService = notificationService;
        }

        public async Task<List<RoleModel>> GetRolesByScopeAsync(string scope)
        {
            var dtos = await _permissionApiClient.GetRolesByScopeAsync(scope);
            return dtos.Select(dto => new RoleModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Scope = dto.Scope,
                Permissions = new ObservableCollection<string>(dto.Permissions)
            }).ToList();
        }

        public async Task<List<PermissionModel>> GetAllPermissionsAsync()
        {
            var dtos = await _permissionApiClient.GetAllPermissionsAsync();
            return dtos.Select(dto => new PermissionModel { Id = dto.Id, Description = dto.Description }).ToList();
        }

        public async Task<List<PermissionModel>> GetPermissionsByScopeAsync(string scope)
        {
            var dtos = await _permissionApiClient.GetPermissionsByScopeAsync(scope);
            return dtos.Select(dto => new PermissionModel { Id = dto.Id, Description = dto.Description }).ToList();
        }

        public async Task<RoleModel?> CreateRoleAsync(string name, string description, string scope)
        {
            var request = new CreateRoleRequest(name, description, scope);
            var response = await _permissionApiClient.CreateRoleAsync(request);

            if (response != null)
            {
                _notificationService.ShowMessage("Role created successfully!", StatusMessageType.Success);
                return new RoleModel { Id = response.RoleId, Name = name, Description = description };
            }
            return null;
        }

        public async Task UpdateRoleAsync(Guid roleId, string name, string description)
        {
            var request = new UpdateRoleRequest(name, description);
            await _permissionApiClient.UpdateRoleAsync(roleId, request);
            _notificationService.ShowMessage("Role updated successfully!", StatusMessageType.Success);
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            await _permissionApiClient.DeleteRoleAsync(roleId);
            _notificationService.ShowMessage("Role deleted successfully!", StatusMessageType.Success);
        }

        public async Task AddPermissionToRoleAsync(Guid roleId, string permissionId)
        {
            var request = new AddPermissionToRoleRequest(permissionId);
            await _permissionApiClient.AddPermissionToRoleAsync(roleId, request);
        }

        public async Task RemovePermissionFromRoleAsync(Guid roleId, string permissionId)
        {
            await _permissionApiClient.RemovePermissionFromRoleAsync(roleId, permissionId);
        }

        public async Task<UserAssignmentsModel> GetAssignmentsForUserInScopeAsync(Guid userId, string scope)
        {
            var dto = await _permissionApiClient.GetAssignmentsForUserInScopeAsync(userId, scope);

            var result = new UserAssignmentsModel
            {
                AssignedRoles = dto.AssignedRoles.Select(r => new RoleModel
                {
                    Id = r.RoleId,
                    Name = r.Name,
                    Permissions = new ObservableCollection<string>(r.PermissionsInRole)
                }).ToList(),

                DirectPermissions = dto.DirectPermissions.Select(p => new PermissionModel
                {
                    Id = p.PermissionId,
                    Description = p.Description
                }).ToList()
            };

            return result;
        }

        public async Task AssignRoleToUserAsync(Guid userId, Guid roleId, string scope)
        {
            var request = new AssignRoleToUserRequest(userId, roleId, scope);
            await _permissionApiClient.AssignRoleToUserAsync(request);
            _notificationService.ShowMessage("Role assigned successfully!", Messages.StatusMessageType.Success);
        }

        public async Task RemoveRoleFromUserAsync(Guid userId, Guid roleId, string scope)
        {
            var request = new RemoveRoleFromUserRequest(userId, roleId, scope);
            await _permissionApiClient.RemoveRoleFromUserAsync(request);
            _notificationService.ShowMessage("Role removed successfully.", Messages.StatusMessageType.Success);
        }

        public async Task AssignDirectPermissionToUserAsync(Guid userId, string permissionId, string scope)
        {
            var request = new AssignDirectPermissionToUserRequest(userId, permissionId, scope);
            await _permissionApiClient.AssignDirectPermissionToUserAsync(request);
        }

        public async Task RemoveDirectPermissionFromUserAsync(Guid userId, string permissionId, string scope)
        {
            var request = new RemoveDirectPermissionFromUserRequest(userId, permissionId, scope);
            await _permissionApiClient.RemoveDirectPermissionFromUserAsync(request);
        }
    }
}
