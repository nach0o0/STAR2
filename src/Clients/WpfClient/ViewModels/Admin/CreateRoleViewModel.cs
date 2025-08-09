using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public CreateRoleViewModel(IPermissionAdminService permissionAdminService, string scope)
        {
            _permissionAdminService = permissionAdminService;
            _scope = scope;
        }

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private async Task Submit()
        {
            await ExecuteCommandAsync(async () =>
            {
                await _permissionAdminService.CreateRoleAsync(NewRoleName, NewRoleDescription, _scope);
                NewRoleName = string.Empty;
                NewRoleDescription = string.Empty;
            });
        }
        private bool CanSubmit() => !string.IsNullOrWhiteSpace(NewRoleName);
    }
}
