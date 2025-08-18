using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.AttendanceService
{
    public record WorkModelDeletedIntegrationEvent
    {
        public Guid WorkModelId { get; init; }
        public Guid EmployeeGroupId { get; init; }
    }
}
