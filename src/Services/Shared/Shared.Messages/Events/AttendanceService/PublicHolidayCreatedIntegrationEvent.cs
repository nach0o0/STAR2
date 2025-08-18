using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.AttendanceService
{
    public record PublicHolidayCreatedIntegrationEvent
    {
        public Guid PublicHolidayId { get; init; }
        public DateTime Date { get; init; }
        public string Name { get; init; }
        public Guid EmployeeGroupId { get; init; }
    }
}
