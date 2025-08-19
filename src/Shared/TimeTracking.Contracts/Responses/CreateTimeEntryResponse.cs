using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Contracts.Responses
{
    public record CreateTimeEntryResponse(
        Guid TimeEntryId,
        Guid? AccessKey
    );
}
