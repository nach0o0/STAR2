using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.AttendanceService
{
    public record AttendanceTypeDeletedIntegrationEvent
    {
        public Guid AttendanceTypeId { get; init; }
        public Guid EmployeeGroupId { get; init; }
    }
}
