using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceSummaryForEmployee
{
    public record GetAttendanceSummaryForEmployeeQueryResult(
        Guid AttendanceTypeId,
        string Name,
        string Color,
        int Count
    );
}
