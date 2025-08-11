using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class AssignRoleDialogViewModel : ViewModelBase
    {
        private readonly IPermissionAdminService _permissionAdminService;
        private readonly UserModel _user;
        private readonly string _scope;

        [ObservableProperty]
        private ObservableCollection<RoleModel> _availableRoles = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AssignRoleCommand))]
        private RoleModel? _selectedRole;

        // Event, das signalisiert, dass der Dialog geschlossen werden soll.
        public event Action? CloseRequested;

        // Event, das die neu zugewiesene Rolle an den Aufrufer zurückgibt.
        public event Action<RoleModel>? RoleAssigned;

        public AssignRoleDialogViewModel(
            IPermissionAdminService permissionAdminService,
            UserModel user,
            string scope)
        {
            _permissionAdminService = permissionAdminService;
            _user = user;
            _scope = scope;

            LoadAvailableRolesCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadAvailableRolesAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var roles = await _permissionAdminService.GetRolesByScopeAsync(_scope);
                AvailableRoles = new ObservableCollection<RoleModel>(roles);
            });
        }

        [RelayCommand(CanExecute = nameof(CanAssignRole))]
        private async Task AssignRoleAsync()
        {
            if (SelectedRole is null) return;

            await ExecuteCommandAsync(async () =>
            {
                await _permissionAdminService.AssignRoleToUserAsync(_user.Id, SelectedRole.Id, _scope);

                RoleAssigned?.Invoke(SelectedRole);
                CloseRequested?.Invoke();
            });
        }
        private bool CanAssignRole() => SelectedRole != null;

        [RelayCommand]
        private void Cancel()
        {
            CloseRequested?.Invoke();
        }
    }
}
