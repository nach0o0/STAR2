using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelsByEmployeeGroup
{
    public record GetWorkModelsByEmployeeGroupQuery(Guid EmployeeGroupId)
        : IRequest<List<GetWorkModelsByEmployeeGroupQueryResult>>;
}
