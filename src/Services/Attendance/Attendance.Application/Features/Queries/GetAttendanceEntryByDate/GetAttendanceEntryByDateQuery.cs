using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceEntryByDate
{
    public record GetAttendanceEntryByDateQuery(
        Guid EmployeeId,
        DateTime Date) : IRequest<GetAttendanceEntryByDateQueryResult?>;
}
