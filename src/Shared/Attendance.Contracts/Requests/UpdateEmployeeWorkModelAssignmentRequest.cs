using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Contracts.Requests
{
    public record UpdateEmployeeWorkModelAssignmentRequest(
        DateTime? ValidFrom,
        DateTime? ValidTo
    );
}
