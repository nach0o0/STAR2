using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelAssignmentsForEmployeeGroup
{
    public record GetWorkModelAssignmentsForEmployeeGroupQuery(Guid EmployeeGroupId)
        : IRequest<List<GetWorkModelAssignmentsForEmployeeGroupQueryResult>>;
}
