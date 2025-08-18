using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetPublicHolidaysByDateRange
{
    public record GetPublicHolidaysByDateRangeQueryResult(
        Guid Id,
        string Name,
        DateTime Date,
        Guid EmployeeGroupId
    );
}
