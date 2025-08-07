using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.UserState;

namespace WpfClient.Services.Application.MyEmployeeProfile
{
    public partial class MyEmployeeProfileService : IMyEmployeeProfileService
    {
        private readonly IOrganizationApiClient _organizationApiClient;
        private readonly IUserStateService _userStateService;

        public MyEmployeeProfileService(
            IOrganizationApiClient organizationApiClient,
            IUserStateService userStateService)
        {
            _organizationApiClient = organizationApiClient;
            _userStateService = userStateService;
        }

        public async Task CreateMyProfileAsync(string firstName, string lastName)
        {
            // 1. Aktion ausführen
            await _organizationApiClient.CreateMyEmployeeProfileAsync(new(firstName, lastName));

            // 2. Zentralen Zustand aktualisieren lassen
            await _userStateService.RefreshProfileAsync();
        }

        public async Task UpdateMyProfileAsync(string firstName, string lastName)
        {
            // 1. Aktion ausführen
            await _organizationApiClient.UpdateMyEmployeeProfileAsync(new(firstName, lastName));

            // 2. Zentralen Zustand aktualisieren lassen
            await _userStateService.RefreshProfileAsync();
        }
    }
}
