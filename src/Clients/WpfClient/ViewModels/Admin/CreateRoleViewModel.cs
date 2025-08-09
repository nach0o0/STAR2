using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class CreateRoleViewModel : ViewModelBase
    {
        private readonly IPermissionAdminService _permissionAdminService;
        private readonly string _scope;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _newRoleName = string.Empty;

        [ObservableProperty]
        private string _newRoleDescription = string.Empty;

        [ObservableProperty]
        private RoleModel? _newlyCreatedRole;

        public event Action? CancelRequested;

        public CreateRoleViewModel(IPermissionAdminService permissionAdminService, string scope)
        {
            _permissionAdminService = permissionAdminService;
            _scope = scope;
        }

        [RelayCommand]
        private void Cancel()
        {
            CancelRequested?.Invoke();
        }

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private async Task<RoleModel?> Submit()
        {
            RoleModel? newlyCreatedRole = null;
            await ExecuteCommandAsync(async () =>
            {
                var newRole = await _permissionAdminService.CreateRoleAsync(NewRoleName, NewRoleDescription, _scope);
                if (newRole != null)
                {
                    NewlyCreatedRole = newRole;
                    //NewRoleName = string.Empty;
                    //NewRoleDescription = string.Empty;
                }
            });
            return newlyCreatedRole;
        }
        private bool CanSubmit() => !string.IsNullOrWhiteSpace(NewRoleName);
    }
}
