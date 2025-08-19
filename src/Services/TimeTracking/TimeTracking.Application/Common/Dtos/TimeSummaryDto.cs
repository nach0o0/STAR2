using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracking.Application.Common.Dtos
{
    public record TimeSummaryDto(Guid GroupingId, string GroupingName, decimal TotalHours);
}
