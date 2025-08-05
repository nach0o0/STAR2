using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService
{
    public record UserAssignedToEmployeeIntegrationEvent
    {
        public Guid EmployeeId { get; init; }
        public Guid UserId { get; init; }
    }
}
