using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeTrackingSummary
{
    public record GetTimeTrackingSummaryQueryResult(
        Guid GroupingId,
        string GroupingName,
        decimal TotalHours
    );
}
