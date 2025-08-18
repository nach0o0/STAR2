using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Events.EmployeeWorkModels
{
    public record EmployeeWorkModelAssignedEvent(EmployeeWorkModel Assignment) : INotification;
}
