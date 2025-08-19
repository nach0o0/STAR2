using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Requests
{
    public record GetMyTimeEntriesRequest(
        DateTime StartDate,
        DateTime EndDate,
        List<Guid>? AccessKeys
    );
}
