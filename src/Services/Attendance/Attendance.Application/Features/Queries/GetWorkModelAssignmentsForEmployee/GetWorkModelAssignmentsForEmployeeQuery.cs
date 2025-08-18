using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployee
{
    public record GetWorkModelAssignmentsForEmployeeQuery(Guid EmployeeId)
        : IRequest<List<GetWorkModelAssignmentsForEmployeeQueryResult>>;
}
