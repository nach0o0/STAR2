using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceTypesByEmployeeGroup
{
    public record GetAttendanceTypesByEmployeeGroupQuery(Guid EmployeeGroupId)
        : IRequest<List<GetAttendanceTypesByEmployeeGroupQueryResult>>;
}
