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
    public partial class ManageDirectPermissionsViewModel : ViewModelBase
    {
        private readonly IPermissionAdminService _permissionAdminService;
        private readonly UserModel _user;
        private readonly string _scope;
        private readonly List<string> _initialPermissions;

        public string UserName => _user.DisplayName;

        [ObservableProperty]
        private ObservableCollection<SelectablePermissionModel> _availablePermissions = new();

        public event System.Action? OnSave;
        public event System.Action? OnCancel;

        public ManageDirectPermissionsViewModel(
            IPermissionAdminService permissionAdminService,
            UserModel user,
            string scope,
            List<PermissionModel> directPermissions)
        {
            _permissionAdminService = permissionAdminService;
            _user = user;
            _scope = scope;
            _initialPermissions = directPermissions.Select(p => p.Id).ToList();

            LoadAvailablePermissionsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadAvailablePermissionsAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var allPermissions = await _permissionAdminService.GetPermissionsByScopeAsync(_scope);
                var selectableItems = allPermissions.Select(p =>
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
                    await _permissionAdminService.AssignDirectPermissionToUserAsync(_user.Id, permissionId, _scope);
                }

                foreach (var permissionId in permissionsToRemove)
                {
                    await _permissionAdminService.RemoveDirectPermissionFromUserAsync(_user.Id, permissionId, _scope);
                }

                // Signalisiere, dass die Arbeit getan ist, damit der Orchestrator die Ansicht wechseln kann.
                OnSave?.Invoke();
            });
        }

        [RelayCommand]
        private void Cancel()
        {
            OnCancel?.Invoke();
        }
    }
}
