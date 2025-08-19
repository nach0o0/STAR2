using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByGroup
{
    public record GetTimeEntriesByGroupQueryResult(
        Guid Id,
        Guid EmployeeId,
        DateTime EntryDate,
        decimal Hours,
        string? Description,
        Guid? CostObjectId,
        string? CostObjectName // Wichtig für die UI
    );
}
