using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.OrganizationService
{
    public record EmployeeGroupDeletedIntegrationEvent
    {
        public Guid EmployeeGroupId { get; init; }
    }
}
