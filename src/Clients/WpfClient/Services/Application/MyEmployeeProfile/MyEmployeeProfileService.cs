using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Messages;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Notification;
using WpfClient.Services.Application.UserState;

namespace WpfClient.Services.Application.MyEmployeeProfile
{
    public partial class MyEmployeeProfileService : IMyEmployeeProfileService
    {
        private readonly IOrganizationApiClient _organizationApiClient;
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;

        public MyEmployeeProfileService(
            IOrganizationApiClient organizationApiClient,
            IAuthService authService,
            INotificationService notificationService)
        {
            _organizationApiClient = organizationApiClient;
            _authService = authService;
            _notificationService = notificationService;
        }

        public async Task CreateMyProfileAsync(string firstName, string lastName)
        {
            // 1. Aktion ausführen
            await _organizationApiClient.CreateMyEmployeeProfileAsync(new(firstName, lastName));
            _notificationService.ShowMessage("Profile created successfully!", StatusMessageType.Success);
            // 2. Eine komplett neue Session anfordern. 
            //    Dies holt ein neues Access Token, das jetzt den employee_id-Claim enthält,
            //    und löst über den Messenger ein Update im UserStateService aus.
            await _authService.TryInitializeSessionAsync();
        }

        public async Task UpdateMyProfileAsync(string firstName, string lastName)
        {
            // 1. Aktion ausführen
            await _organizationApiClient.UpdateMyEmployeeProfileAsync(new(firstName, lastName));
            _notificationService.ShowMessage("Profile updated successfully!", StatusMessageType.Success);

            // Beim Update ist keine neue Session nötig, nur ein Refresh der lokalen Daten.
            // TryInitializeSessionAsync würde auch hier funktionieren, ist aber nicht zwingend.
            await _authService.TryInitializeSessionAsync();
        }
    }
}
