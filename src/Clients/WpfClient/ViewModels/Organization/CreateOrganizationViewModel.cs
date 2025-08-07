using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Organization.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Notification;
using WpfClient.Services.Application.Organization;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Organization
{
    public partial class CreateOrganizationViewModel : ViewModelBase
    {
        private readonly IOrganizationService _organizationService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateOrganizationCommand))]
        private string _organizationName = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateOrganizationCommand))]
        private string _organizationAbbreviation = string.Empty;

        public CreateOrganizationViewModel(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [RelayCommand(CanExecute = nameof(CanCreateOrganization))]
        private async Task CreateOrganization()
        {
            await ExecuteCommandAsync(async () =>
            {
                var success = await _organizationService.CreateOrganizationAsync(OrganizationName, OrganizationAbbreviation);

                if (success)
                {
                    OrganizationName = string.Empty;
                    OrganizationAbbreviation = string.Empty;
                }
            });
        }

        private bool CanCreateOrganization() =>
            !string.IsNullOrWhiteSpace(OrganizationName) &&
            !string.IsNullOrWhiteSpace(OrganizationAbbreviation);
    }
}