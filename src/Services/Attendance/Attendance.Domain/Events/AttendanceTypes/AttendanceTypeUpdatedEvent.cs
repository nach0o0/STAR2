using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Events.AttendanceTypes
{
    public record AttendanceTypeUpdatedEvent(AttendanceType AttendanceType) : INotification;
}
