using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Requests
{
    public record UpdateTimeEntryRequest(
        DateTime? EntryDate,
        Guid? CostObjectId,
        decimal? Hours,
        string? Description,
        Guid? AccessKey
    );
}
