using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Common.Enums;

namespace TimeTracking.Application.Features.Queries.GetTimeTrackingSummary
{
    public record GetTimeTrackingSummaryQuery(
        Guid EmployeeGroupId,
        DateTime StartDate,
        DateTime EndDate,
        SummaryGroupBy GroupBy
    ) : IRequest<List<GetTimeTrackingSummaryQueryResult>>;
}
