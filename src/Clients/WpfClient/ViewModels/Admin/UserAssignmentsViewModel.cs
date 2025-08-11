using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.Services.Application.UserAdmin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class UserAssignmentsViewModel : ViewModelBase
    {
        private readonly IPermissionAdminService _permissionAdminService;
        private readonly IUserAdminService _userAdminService;
        private readonly string _scope;

        [ObservableProperty]
        private UserModel _user;

        [ObservableProperty]
        private ObservableCollection<RoleModel> _assignedRoles = new();

        [ObservableProperty]
        private ObservableCollection<PermissionModel> _directPermissions = new();

        [ObservableProperty]
        private bool _isResetPasswordFormVisible;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitResetPasswordCommand))]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitResetPasswordCommand))]
        private string _confirmNewPassword = string.Empty;

        // Berechtigungen zur Steuerung der UI-Elemente in der View
        public bool CanAssignRoles { get; }
        public bool CanAssignDirectPermissions { get; }
        public bool CanResetPassword { get; }
        public bool CanReadAssignments { get; }

        public event Action? AssignRoleRequested;
        public event Action? ManageDirectPermissionsRequested;

        public UserAssignmentsViewModel(
            IPermissionService permissionService,
            IPermissionAdminService permissionAdminService,
            IUserAdminService userAdminService,
            UserModel user,
            string scope)
        {
            _user = user;
            _scope = scope;
            _permissionAdminService = permissionAdminService;
            _userAdminService = userAdminService;

            // Granulare Prüfung der Berechtigungen für die Aktionen in dieser Ansicht
            CanReadAssignments = permissionService.HasPermissionInScope(PermissionKeys.AssignmentRead, scope);
            CanAssignRoles = permissionService.HasPermissionInScope(PermissionKeys.PermissionAssignRole, scope);
            CanAssignDirectPermissions = permissionService.HasPermissionInScope(PermissionKeys.PermissionAssignDirect, scope);
            CanResetPassword = permissionService.HasPermissionInScope(PermissionKeys.PrivilegedResetPassword, scope);

            // Lade die Zuweisungen, wenn der Admin sie sehen darf.
            if (CanReadAssignments)
            {
                LoadAssignmentsCommand.Execute(null);
            }
        }

        [RelayCommand(CanExecute = nameof(CanResetPassword))]
        private void ShowResetPasswordForm()
        {
            IsResetPasswordFormVisible = true;
        }

        [RelayCommand]
        private async Task LoadAssignmentsAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var assignments = await _permissionAdminService.GetAssignmentsForUserInScopeAsync(_user.Id, _scope);
                AssignedRoles = new ObservableCollection<RoleModel>(assignments.AssignedRoles);
                DirectPermissions = new ObservableCollection<PermissionModel>(assignments.DirectPermissions);
            });
        }

        [RelayCommand]
        private void CancelResetPassword()
        {
            NewPassword = string.Empty;
            ConfirmNewPassword = string.Empty;
            IsResetPasswordFormVisible = false;
        }

        // Dieser Befehl führt den eigentlichen Reset durch
        [RelayCommand(CanExecute = nameof(CanSubmitResetPassword))]
        private async Task SubmitResetPasswordAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                await _userAdminService.PrivilegedResetPasswordAsync(User.Id, NewPassword);
                // Nach Erfolg das Formular schließen
                CancelResetPassword();
            });
        }
        private bool CanSubmitResetPassword()
        {
            return !string.IsNullOrWhiteSpace(NewPassword) && NewPassword == ConfirmNewPassword;
        }

        [RelayCommand(CanExecute = nameof(CanAssignRoles))]
        private void RequestAssignRole()
        {
            // Löst das Event aus, auf das der UserManagementViewModel (Orchestrator) lauschen wird.
            AssignRoleRequested?.Invoke();
        }

        [RelayCommand(CanExecute = nameof(CanAssignDirectPermissions))]
        private void RequestManageDirectPermissions()
        {
            ManageDirectPermissionsRequested?.Invoke();
        }

        [RelayCommand(CanExecute = nameof(CanAssignRoles))]
        private async Task RemoveRoleAsync(RoleModel roleToRemove)
        {
            await ExecuteCommandAsync(async () =>
            {
                await _permissionAdminService.RemoveRoleFromUserAsync(User.Id, roleToRemove.Id, _scope);
                // Entferne die Rolle aus der lokalen Liste, um die UI sofort zu aktualisieren.
                AssignedRoles.Remove(roleToRemove);
            });
        }
    }
}
