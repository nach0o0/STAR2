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
    public partial class ManageRolePermissionsViewModel : ViewModelBase
    {
        private readonly IPermissionAdminService _permissionAdminService;
        private readonly RoleModel _role;
        private readonly List<string> _initialPermissions;

        public string RoleName => _role.Name;

        [ObservableProperty]
        private ObservableCollection<SelectablePermissionModel> _availablePermissions = new();

        public event System.Action<RoleModel>? OnSave;
        public event System.Action? OnCancel;

        public ManageRolePermissionsViewModel(IPermissionAdminService permissionAdminService, RoleModel role, string scope)
        {
            _permissionAdminService = permissionAdminService;
            _role = role;
            // Speichere den initialen Zustand, um Änderungen zu erkennen
            _initialPermissions = new List<string>(_role.Permissions);

            LoadAvailablePermissionsCommand.Execute(scope);
        }

        [RelayCommand]
        private async Task LoadAvailablePermissionsAsync(string scope)
        {
            await ExecuteCommandAsync(async () =>
            {
                List<PermissionModel> allPermissionsForScope;

                // KORREKTE LOGIK: Prüfe den Scope der Rolle, die wir bearbeiten.
                if (_role.Scope == null)
                {
                    // Wenn die Rolle global ist, lade ALLE existierenden Berechtigungen.
                    allPermissionsForScope = await _permissionAdminService.GetAllPermissionsAsync();
                }
                else
                {
                    // Wenn die Rolle einen Scope hat, lade nur die für diesen Scope relevanten Berechtigungen.
                    allPermissionsForScope = await _permissionAdminService.GetPermissionsByScopeAsync(_role.Scope);
                }

                var selectableItems = allPermissionsForScope.Select(p =>
                    new SelectablePermissionModel(p, _initialPermissions.Contains(p.Id)));
                AvailablePermissions = new ObservableCollection<SelectablePermissionModel>(selectableItems);
            });
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var currentSelectedIds = AvailablePermissions.Where(p => p.IsSelected).Select(p => p.Permission.Id).ToList();

                var permissionsToAdd = currentSelectedIds.Except(_initialPermissions).ToList();
                var permissionsToRemove = _initialPermissions.Except(currentSelectedIds).ToList();

                foreach (var permissionId in permissionsToAdd)
                {
                    await _permissionAdminService.AddPermissionToRoleAsync(_role.Id, permissionId);
                }

                foreach (var permissionId in permissionsToRemove)
                {
                    await _permissionAdminService.RemovePermissionFromRoleAsync(_role.Id, permissionId);
                }

                // Aktualisiere das ursprüngliche RoleModel und signalisiere den Erfolg.
                _role.Permissions = new ObservableCollection<string>(currentSelectedIds);
                OnSave?.Invoke(_role);
            });
        }

        [RelayCommand]
        private void Cancel()
        {
            OnCancel?.Invoke();
        }
    }
}
