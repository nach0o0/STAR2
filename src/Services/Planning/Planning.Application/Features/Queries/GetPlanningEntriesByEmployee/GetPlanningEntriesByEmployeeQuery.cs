using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByEmployee
{
    public record GetPlanningEntriesByEmployeeQuery(
        Guid EmployeeId,
        Guid EmployeeGroupId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<List<GetPlanningEntriesByEmployeeQueryResult>>;
}
