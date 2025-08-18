using Attendance.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Events.AttendanceEntries
{
    public record AttendanceEntryDeletedEvent(AttendanceEntry Entry) : INotification;
}
