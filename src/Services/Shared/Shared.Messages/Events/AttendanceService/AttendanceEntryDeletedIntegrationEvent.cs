using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.AttendanceService
{
    public record AttendanceEntryDeletedIntegrationEvent
    {
        public Guid AttendanceEntryId { get; init; }
        public Guid EmployeeId { get; init; }
        public DateTime Date { get; init; }
    }
}
