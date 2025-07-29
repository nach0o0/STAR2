using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.EmployeeGroups
{
    public record EmployeeGroupCreatedIntegrationEvent
    {
        public Guid EmployeeGroupId { get; init; }
        public string Name { get; init; }
        public Guid LeadingOrganizationId { get; init; }
        public Guid CreatorUserId { get; init; }
    }
}
