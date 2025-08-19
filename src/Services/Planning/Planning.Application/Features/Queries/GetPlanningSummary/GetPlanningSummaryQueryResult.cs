using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningSummary
{
    public record GetPlanningSummaryQueryResult(
        Guid GroupingId,
        string GroupingName,
        decimal TotalHours
    );
}
