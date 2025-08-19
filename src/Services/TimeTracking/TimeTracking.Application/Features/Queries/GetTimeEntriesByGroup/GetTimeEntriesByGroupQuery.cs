using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByGroup
{
    public record GetTimeEntriesByGroupQuery(
        Guid EmployeeGroupId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<List<GetTimeEntriesByGroupQueryResult>>;
}
