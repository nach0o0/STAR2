using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Responses
{
    public record TimeEntryResponse(
        Guid Id,
        DateTime EntryDate,
        decimal Hours,
        decimal HourlyRate,
        string? Description,
        Guid? CostObjectId,
        Guid EmployeeGroupId,
        Guid EmployeeId
    );
}
