using Shared.Application.Interfaces.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Commands.UpdateAttendanceType
{
    public record UpdateAttendanceTypeCommand(
        Guid AttendanceTypeId,
        string? Name,
        string? Abbreviation,
        bool? IsPresent,
        string? Color) : ICommand;
}
