using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetMyTimeEntriesByDateRange
{
    public record GetMyTimeEntriesByDateRangeQuery(
        DateTime StartDate,
        DateTime EndDate,
        List<Guid>? AccessKeys
    ) : IRequest<List<GetMyTimeEntriesByDateRangeQueryResult>>;
}
