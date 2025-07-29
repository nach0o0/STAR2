using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.Employees
{
    public record EmployeeAssignedToOrganizationIntegrationEvent
    {
        public Guid EmployeeId { get; init; }
        public Guid OrganizationId { get; init; }
        public Guid UserId { get; init; }
    }
}
