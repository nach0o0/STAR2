using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Contracts.Responses
{
    public record WorkModelAssignmentResponse(
        Guid AssignmentId,
        Guid EmployeeId,
        Guid WorkModelId,
        string WorkModelName,
        DateTime ValidFrom,
        DateTime? ValidTo
    );
}
