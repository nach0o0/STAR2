using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Contracts.Responses
{
    public record WorkModelResponse(
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
