using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.CalculateTargetWorkingHours
{
    public record CalculateTargetWorkingHoursQueryResult(
        decimal TargetHours
    );
}
