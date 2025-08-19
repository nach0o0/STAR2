using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Responses
{
    public record TimeEntriesByGroupResponse(
        Guid Id,
        Guid EmployeeId,
        DateTime EntryDate,
        decimal Hours,
        string? Description,
        Guid? CostObjectId,
        string? CostObjectName
    );
}
