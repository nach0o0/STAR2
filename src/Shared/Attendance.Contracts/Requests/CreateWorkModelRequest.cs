using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Contracts.Requests
{
    public record CreateWorkModelRequest(
        string Name,
        Guid EmployeeGroupId,
        decimal MondayHours,
        decimal TuesdayHours,
        decimal WednesdayHours,
        decimal ThursdayHours,
        decimal FridayHours,
        decimal SaturdayHours,
        decimal SundayHours
    );
}
