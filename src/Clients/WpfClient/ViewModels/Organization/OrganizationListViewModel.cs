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
using WpfClient.Services.Application.Organization;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Organization
{
    public partial class OrganizationListViewModel : ViewModelBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IPermissionService _permissionService;

        [ObservableProperty]
        private ObservableCollection<OrganizationModel> _organizations = new();

        [ObservableProperty]
        private OrganizationModel? _selectedOrganization;

        [ObservableProperty]
        private bool _createNewRequested;

        public bool CanCreateTopLevelOrganization { get; }

        public OrganizationListViewModel(
            IOrganizationService organizationService,
            IPermissionService permissionService)
        {
            _organizationService = organizationService;
            _permissionService = permissionService;

            CanCreateTopLevelOrganization = _permissionService.HasPermissionInScope(PermissionKeys.OrganizationCreate, "global");

            LoadOrganizationsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadOrganizationsAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var orgs = await _organizationService.GetRelevantOrganizationsAsync();
                Organizations = new ObservableCollection<OrganizationModel>(orgs);
                SelectedOrganization = null;
            });
        }

        [RelayCommand(CanExecute = nameof(CanCreateTopLevelOrganization))]
        private void RequestCreateNew()
        {
            // Setzt das Flag, worauf der Orchestrator (OrganizationWorkspaceViewModel) reagieren wird.
            CreateNewRequested = true;
        }
    }
}
