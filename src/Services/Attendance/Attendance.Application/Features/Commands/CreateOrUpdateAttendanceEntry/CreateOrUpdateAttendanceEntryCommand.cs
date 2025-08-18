using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.CreateOrUpdateAttendanceEntry
{
    public record CreateOrUpdateAttendanceEntryCommand(
        Guid EmployeeId,
        DateTime Date,
        Guid AttendanceTypeId,
        string? Note) : ICommand<Guid>;
}
