using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Auth;

namespace WpfClient.Services.Application.MyEmployeeProfile
{
    public partial class MyEmployeeProfileService : ObservableObject, IMyEmployeeProfileService
    {
        private readonly IOrganizationApiClient _organizationApiClient;

        [ObservableProperty]
        private MyEmployeeProfileModel? _currentProfile;

        public MyEmployeeProfileService(IOrganizationApiClient organizationApiClient)
        {
            _organizationApiClient = organizationApiClient;

            ClientAppMediator.UserLoggedIn += LoadProfileAsync;
            ClientAppMediator.UserLoggedOut += ClearProfile;
        }

        public async Task LoadProfileAsync()
        {
            var profileDto = await _organizationApiClient.GetMyEmployeeProfileAsync();
            if (profileDto is not null)
            {
                CurrentProfile = new MyEmployeeProfileModel
                {
                    EmployeeId = profileDto.EmployeeId,
                    FirstName = profileDto.FirstName,
                    LastName = profileDto.LastName
                };
            }
        }

        public async Task<Guid?> CreateMyProfileAsync(string firstName, string lastName)
        {
            var response = await _organizationApiClient.CreateMyEmployeeProfileAsync(new(firstName, lastName));
            if (response is null) return null;

            await ClientAppMediator.NotifyProfileCreatedAsync();

            await LoadProfileAsync(); // Lade das Profil neu, um den Zustand zu aktualisieren
            return response?.EmployeeId;
        }

        public async Task UpdateMyProfileAsync(string firstName, string lastName)
        {
            await _organizationApiClient.UpdateMyEmployeeProfileAsync(new(firstName, lastName));
            await LoadProfileAsync(); // Lade das Profil neu
        }

        public void ClearProfile()
        {
            CurrentProfile = null;
        }
    }
}
