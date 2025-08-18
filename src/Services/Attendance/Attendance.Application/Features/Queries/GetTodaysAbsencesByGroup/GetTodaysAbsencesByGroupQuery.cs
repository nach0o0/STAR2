using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetTodaysAbsencesByGroup
{
    public record GetTodaysAbsencesByGroupQuery(
        Guid EmployeeGroupId,
        DateTime Date) : IRequest<List<GetTodaysAbsencesByGroupQueryResult>>;
}
