using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Responses
{
    public record TimeEntriesByCostObjectResponse(
        Guid Id,
        Guid EmployeeId,
        DateTime EntryDate,
        decimal Hours,
        string? Description
    );
}
