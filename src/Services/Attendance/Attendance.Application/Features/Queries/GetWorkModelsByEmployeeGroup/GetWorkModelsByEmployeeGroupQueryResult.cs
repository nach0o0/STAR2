using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetWorkModelsByEmployeeGroup
{
    public record GetWorkModelsByEmployeeGroupQueryResult(
        Guid Id,
        string Name,
        decimal MondayHours,
        decimal TuesdayHours,
        decimal WednesdayHours,
        decimal ThursdayHours,
        decimal FridayHours,
        decimal SaturdayHours,
        decimal SundayHours
    );
}
