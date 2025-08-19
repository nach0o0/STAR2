using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByGroup
{
    public record GetPlanningEntriesByGroupQuery(
        Guid EmployeeGroupId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<List<GetPlanningEntriesByGroupQueryResult>>;
}
