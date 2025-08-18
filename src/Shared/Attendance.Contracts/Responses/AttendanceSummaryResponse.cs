using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Contracts.Responses
{
    public record AttendanceSummaryItem(
        Guid AttendanceTypeId,
        string Name,
        string Color,
        int Count
    );

    public record AttendanceSummaryResponse(
        List<AttendanceSummaryItem> SummaryItems
    );
}
