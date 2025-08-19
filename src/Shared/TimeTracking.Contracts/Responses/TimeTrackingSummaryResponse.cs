using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Responses
{
    public record TimeTrackingSummaryResponse(
        Guid GroupingId,
        string GroupingName,
        decimal TotalHours
    );
}
