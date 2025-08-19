using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetMyTimeEntriesByDateRange
{
    public record GetMyTimeEntriesByDateRangeQueryResult(
        Guid Id,
        DateTime EntryDate,
        decimal Hours,
        string? Description,
        Guid? CostObjectId,
        bool IsAnonymized,
        Guid? AccessKey
    );
}
