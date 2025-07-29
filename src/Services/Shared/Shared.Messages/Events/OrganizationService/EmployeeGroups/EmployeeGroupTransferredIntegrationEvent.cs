using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.EmployeeGroups
{
    public record EmployeeGroupTransferredIntegrationEvent
    {
        public Guid EmployeeGroupId { get; init; }
        public Guid SourceOrganizationId { get; init; }
        public Guid DestinationOrganizationId { get; init; }
    }
}
