using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Contracts.Responses
{
    public record AttendanceTypeResponse(
        Guid Id,
        string Name,
        string Abbreviation,
        bool IsPresent,
        string Color
    );
}
