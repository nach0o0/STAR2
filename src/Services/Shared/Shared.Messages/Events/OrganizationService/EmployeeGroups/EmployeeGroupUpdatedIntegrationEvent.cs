using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.EmployeeGroups
{
    public record EmployeeGroupUpdatedIntegrationEvent
    {
        public Guid EmployeeGroupId { get; init; }
        public string NewName { get; init; }
    }
}
