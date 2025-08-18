using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeGroupByDateRange
{
    public record GetAttendanceEntriesForEmployeeGroupByDateRangeQuery(
        Guid EmployeeGroupId,
        DateTime StartDate,
        DateTime EndDate) : IRequest<List<GetAttendanceEntriesForEmployeeGroupByDateRangeQueryResult>>;
}
