using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService
{
    public record EmployeeCreatedIntegrationEvent
    {
        public Guid EmployeeId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}
