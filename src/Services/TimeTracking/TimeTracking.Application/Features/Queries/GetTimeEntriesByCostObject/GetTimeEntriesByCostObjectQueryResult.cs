using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByCostObject
{
    public record GetTimeEntriesByCostObjectQueryResult(
        Guid Id,
        Guid EmployeeId,
        DateTime EntryDate,
        decimal Hours,
        string? Description
    );
}
