using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService.Employees
{
    public record EmployeeUpdatedIntegrationEvent
    {
        public Guid EmployeeId { get; init; }
        public string NewFirstName { get; init; }
        public string NewLastName { get; init; }
    }
}
