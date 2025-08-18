using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Events.EmployeeWorkModels
{
    public record EmployeeWorkModelUnassignedEvent(Guid EmployeeId, Guid WorkModelId) : INotification;
}
