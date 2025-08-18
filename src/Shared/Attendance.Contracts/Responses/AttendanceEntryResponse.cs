using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Contracts.Responses
{
    public record AttendanceEntryResponse(
        Guid Id,
        DateTime Date,
        Guid AttendanceTypeId,
        string AttendanceTypeName,
        string AttendanceTypeColor,
        string? Note
    );
}
