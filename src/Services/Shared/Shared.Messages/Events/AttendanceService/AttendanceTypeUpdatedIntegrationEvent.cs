using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Events.AttendanceService
{
    public record AttendanceTypeUpdatedIntegrationEvent
    {
        public Guid AttendanceTypeId { get; init; }
        public Guid EmployeeGroupId { get; init; }
        public string Name { get; init; }
        public string Abbreviation { get; init; }
        public bool IsPresent { get; init; }
        public string Color { get; init; }
    }
}
