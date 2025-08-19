using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Contracts.Responses
{
    public record PlanningSummaryResponse(
        Guid GroupingId,
        string GroupingName,
        decimal TotalHours
    );
}
