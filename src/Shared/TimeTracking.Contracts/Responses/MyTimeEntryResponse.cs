using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Responses
{
    public record MyTimeEntryResponse(
        Guid Id,
        DateTime EntryDate,
        decimal Hours,
        string? Description,
        Guid? CostObjectId,
        bool IsAnonymized,
        Guid? AccessKey
    );
}
