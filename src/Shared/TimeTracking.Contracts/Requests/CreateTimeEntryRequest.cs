using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Requests
{
    public record CreateTimeEntryRequest(
        Guid? CostObjectId,
        DateTime EntryDate,
        decimal Hours,
        decimal HourlyRate,
        string? Description,
        bool CreateAnonymously = false
    );
}
