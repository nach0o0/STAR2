using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeByDateRange
{
    public record GetAttendanceEntriesForEmployeeByDateRangeQueryResult(
        Guid Id,
        DateTime Date,
        Guid AttendanceTypeId,
        string AttendanceTypeName,
        string AttendanceTypeColor,
        string? Note
    );
}
