using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetAnonymizedTimeEntry
{
    public record GetAnonymizedTimeEntryQueryResult(
        Guid Id,
        DateTime EntryDate,
        decimal Hours,
        decimal HourlyRate,
        string? Description,
        Guid? CostObjectId,
        Guid EmployeeGroupId
    );
}
