using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService
{
    public record EmployeeOrganizationAssignmentChangedIntegrationEvent
    {
        public Guid UserId { get; init; }
        public Guid EmployeeId { get; init; }
        public Guid? OrganizationId { get; init; } // Ist null, wenn die Zuweisung aufgehoben wird
    }
}
