using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Organization.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Notification;
using WpfClient.Services.Application.Organization;
using WpfClient.Services.Application.OrganizationAdmin;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Organization
{
    public partial class CreateOrganizationViewModel : ViewModelBase
    {
        private readonly IOrganizationAdminService _organizationAdminService;
        private readonly Guid? _parentId;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _newOrganizationName = string.Empty;

        [ObservableProperty]
        private string _newOrganizationAbbreviation = string.Empty;

        public OrganizationModel? NewlyCreatedOrganization { get; private set; }
        public event Action? CancelRequested;

        public CreateOrganizationViewModel(IOrganizationAdminService organizationAdminService, Guid? parentId)
        {
            _organizationAdminService = organizationAdminService;
            _parentId = parentId;
        }

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private async Task Submit()
        {
            await ExecuteCommandAsync(async () =>
            {
                var newOrg = await _organizationAdminService.CreateOrganizationAsync(NewOrganizationName, NewOrganizationAbbreviation, _parentId);
                if (newOrg != null)
                {
                    NewlyCreatedOrganization = newOrg; // Signal für den Orchestrator
                }
            });
        }
        private bool CanSubmit() => !string.IsNullOrWhiteSpace(NewOrganizationName);

        [RelayCommand]
        private void Cancel()
        {
            // TODO: Event für den Orchestrator auslösen, um die Ansicht zu schließen.
        }
    }
}