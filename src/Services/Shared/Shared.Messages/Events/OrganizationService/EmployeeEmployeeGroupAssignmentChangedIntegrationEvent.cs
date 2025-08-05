using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService
{
    public record EmployeeEmployeeGroupAssignmentChangedIntegrationEvent
    {
        public Guid UserId { get; init; }
        public Guid EmployeeId { get; init; }
        public List<Guid> EmployeeGroupIds { get; init; } = new();
    }
}
