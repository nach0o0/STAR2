using Auth.Contracts.Requests;
using Organization.Contracts.Responses;
using Permission.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Notification;

namespace WpfClient.Services.Application.UserAdmin
{
    public class UserAdminService : IUserAdminService
    {
        private readonly IPermissionApiClient _permissionApiClient;
        private readonly IAuthApiClient _authApiClient;
        private readonly IOrganizationApiClient _organizationApiClient;
        private readonly INotificationService _notificationService;

        public UserAdminService(
            IPermissionApiClient permissionApiClient,
            IAuthApiClient authApiClient,
            IOrganizationApiClient organizationApiClient,
            INotificationService notificationService)
        {
            _permissionApiClient = permissionApiClient;
            _authApiClient = authApiClient;
            _organizationApiClient = organizationApiClient;
            _notificationService = notificationService;
        }

        public async Task<List<UserModel>> GetRelevantUsersForScopeAsync(string scope)
        {
            var usersDictionary = new Dictionary<Guid, UserModel>();

            // 1. Hole alle Benutzer, die bereits Berechtigungen in diesem Scope haben, vom PermissionService.
            var assignmentsResponse = await _permissionApiClient.GetAssignmentsByScopeAsync(scope);
            foreach (var userAssignment in assignmentsResponse.UserAssignments)
            {
                if (!usersDictionary.ContainsKey(userAssignment.UserId))
                {
                    usersDictionary[userAssignment.UserId] = new UserModel
                    {
                        Id = userAssignment.UserId,
                        Email = userAssignment.UserEmail,
                        // Hier fehlen FirstName/LastName, was die Notwendigkeit eines Batch-Endpunkts zeigt,
                        // den der PermissionService intern aufrufen kann.
                    };
                }
            }

            List<EmployeeResponse>? members = null;

            // 2. Wenn es ein Organisations-Scope ist, hole zusätzlich alle direkten Mitglieder.
            if (scope.StartsWith("organization:"))
            {
                var orgId = Guid.Parse(scope.Split(':')[1]);
                members = await _organizationApiClient.GetEmployeesByOrganizationAsync(orgId);
            }
            else if (scope.StartsWith("employee-group:"))
            {
                var groupId = Guid.Parse(scope.Split(':')[1]);
                members = await _organizationApiClient.GetEmployeesByEmployeeGroupAsync(groupId);
            }
            if (members != null)
            { 
                foreach (var member in members.Where(m => m.UserId.HasValue))
                {
                    var userModel = new UserModel
                    {
                        Id = member.UserId!.Value,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        // E-Mail fehlt hier, was dasselbe Problem wie oben aufzeigt.
                    };

                    if (usersDictionary.TryGetValue(userModel.Id, out var existingUser))
                    {
                        // Reichere den bereits vorhandenen Benutzer mit den Namen an.
                        existingUser.FirstName = userModel.FirstName;
                        existingUser.LastName = userModel.LastName;
                    }
                    else
                    {
                        usersDictionary[userModel.Id] = userModel;
                    }
                }
            }

            return usersDictionary.Values.OrderBy(u => u.DisplayName).ToList();
        }

        public async Task<UserModel?> FindUserByEmailAsync(string email, string scope)
        {
            try
            {
                var request = new FindUserByEmailRequest(email, scope);
                var userDto = await _permissionApiClient.FindUserForManagementAsync(request);
                return new UserModel
                {
                    Id = userDto.UserId,
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName
                };
            }
            catch (Refit.ApiException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task PrivilegedResetPasswordAsync(Guid userId, string newPassword)
        {
            var request = new PrivilegedResetPasswordRequest(newPassword);
            await _authApiClient.PrivilegedResetPasswordAsync(userId, request);
            _notificationService.ShowMessage("Password has been reset successfully.", Messages.StatusMessageType.Success);
        }
    }
}
