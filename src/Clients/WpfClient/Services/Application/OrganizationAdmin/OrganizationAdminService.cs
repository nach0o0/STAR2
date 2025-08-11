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

namespace WpfClient.Services.Application.OrganizationAdmin
{
    public class OrganizationAdminService : IOrganizationAdminService
    {
        private readonly IOrganizationApiClient _organizationApiClient;
        private readonly INotificationService _notificationService;

        public OrganizationAdminService(IOrganizationApiClient organizationApiClient, INotificationService notificationService)
        {
            _organizationApiClient = organizationApiClient;
            _notificationService = notificationService;
        }

        public async Task<OrganizationModel?> CreateOrganizationAsync(string name, string abbreviation, Guid? parentId)
        {
            var request = new CreateOrganizationRequest(name, abbreviation, parentId);
            var response = await _organizationApiClient.CreateOrganizationAsync(request);

            if (response != null)
            {
                _notificationService.ShowMessage("Organization created successfully!", StatusMessageType.Success);
                return new OrganizationModel { Id = response.OrganizationId, Name = name, IsPrimary = false };
            }
            return null;
        }
    }
}
