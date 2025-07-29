using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.Organizations
{
    public record OrganizationDeletedIntegrationEvent
    {
        public Guid OrganizationId { get; init; }
    }
}
