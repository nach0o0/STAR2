using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Features.Queries.GetTimeEntryById
{
    public record GetTimeEntryByIdQueryResult(
        Guid Id,
        DateTime EntryDate,
        decimal Hours,
        decimal HourlyRate,
        string? Description,
        Guid? CostObjectId,
        Guid EmployeeGroupId,
        Guid EmployeeId // Wichtig, um den Eigentümer zu bestätigen
    );
}
