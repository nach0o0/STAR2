using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Common.Dtos
{
    public record PlanningSummaryDto(Guid GroupingId, decimal TotalHours);
}
