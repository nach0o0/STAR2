using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Organization;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Organization
{
    public partial class OrganizationWorkspaceViewModel : ViewModelBase
    {
        private readonly IPermissionService _permissionService;
        private readonly IOrganizationService _organizationService;

        public CreateOrganizationViewModel CreateOrganizationViewModel { get; }
        public bool CanCreateOrganization { get; }

        [ObservableProperty]
        private ObservableCollection<EmployeeModel> _employees = new();

        public OrganizationWorkspaceViewModel(
            IPermissionService permissionService,
            IOrganizationService organizationService,
            CreateOrganizationViewModel createOrganizationViewModel)
        {
            _permissionService = permissionService;
            _organizationService = organizationService;
            CreateOrganizationViewModel = createOrganizationViewModel;

            // Check permission once when the workspace is loaded
            CanCreateOrganization = _permissionService.HasPermission(PermissionKeys.OrganizationCreate);

            // Lade die Mitarbeiter automatisch, wenn der Workspace betreten wird.
            LoadEmployeesCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadEmployeesAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var employeeList = await _organizationService.GetEmployeesForMyOrganizationAsync();
                Employees = new ObservableCollection<EmployeeModel>(employeeList);
            });
        }
    }
}
