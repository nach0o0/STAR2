using Organization.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Messages;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Notification;
using WpfClient.Services.Application.UserState;

namespace WpfClient.Services.Application.Organization
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationApiClient _organizationApiClient;
        private readonly INotificationService _notificationService;
        private readonly IUserStateService _userStateService;

        public OrganizationService(
            IOrganizationApiClient organizationApiClient, 
            INotificationService notificationService,
            IUserStateService userStateService)
        {
            _organizationApiClient = organizationApiClient;
            _notificationService = notificationService;
            _userStateService = userStateService;
        }

        public async Task<bool> CreateOrganizationAsync(string name, string abbreviation)
        {
            var request = new CreateOrganizationRequest(name, abbreviation, null);

            // Die try-catch Logik für API-Aufrufe gehört hierher, in den Service.
            try
            {
                await _organizationApiClient.CreateOrganizationAsync(request);
                _notificationService.ShowMessage("Organization created successfully!", StatusMessageType.Success);
                return true;
            }
            catch
            {
                // Die ViewModelBase wird die Exception fangen und eine generische Fehlermeldung anzeigen.
                // Spezifischere Fehlerbehandlung könnte hier stattfinden.
                return false;
            }
        }

        public async Task<List<EmployeeModel>> GetEmployeesForMyOrganizationAsync()
        {
            // Finde die ID der primären Organisation des aktuellen Benutzers.
            var organizationId = _userStateService.CurrentUser?.OrganizationId;

            if (!organizationId.HasValue)
            {
                // Wenn der Benutzer keiner Organisation angehört, gib eine leere Liste zurück.
                return new List<EmployeeModel>();
            }

            // Rufe die Mitarbeiter für diese Organisation ab.
            var employeeDtos = await _organizationApiClient.GetEmployeesByOrganizationAsync(organizationId.Value);

            // Wandle die DTOs in die für die UI benötigten Modelle um.
            var employeeModels = employeeDtos.Select(dto => new EmployeeModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            }).ToList();

            return employeeModels;
        }
    }
}
