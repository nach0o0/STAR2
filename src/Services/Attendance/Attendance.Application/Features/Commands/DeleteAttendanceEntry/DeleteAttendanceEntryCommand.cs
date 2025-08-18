using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.DeleteAttendanceEntry
{
    public record DeleteAttendanceEntryCommand(Guid AttendanceEntryId) : ICommand;
}
