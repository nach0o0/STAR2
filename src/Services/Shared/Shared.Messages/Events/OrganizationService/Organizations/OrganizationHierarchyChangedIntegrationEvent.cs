using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.Organizations
{
    public record OrganizationHierarchyChangedIntegrationEvent
    {
        public Guid OrganizationId { get; init; }
        public Guid? NewParentId { get; init; }
    }
}
