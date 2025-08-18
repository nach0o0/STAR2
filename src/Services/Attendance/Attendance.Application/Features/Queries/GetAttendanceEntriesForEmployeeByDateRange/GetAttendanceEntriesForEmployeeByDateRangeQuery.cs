using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeByDateRange
{
    public record GetAttendanceEntriesForEmployeeByDateRangeQuery(
        Guid EmployeeId,
        List<Guid> EmployeeGroupIds,
        DateTime StartDate,
        DateTime EndDate) : IRequest<List<GetAttendanceEntriesForEmployeeByDateRangeQueryResult>>;
}
