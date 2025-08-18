using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.CalculateTargetWorkingHours
{
    public record CalculateTargetWorkingHoursQuery(
        Guid EmployeeId,
        DateTime StartDate,
        DateTime EndDate) : IRequest<CalculateTargetWorkingHoursQueryResult>;
}
